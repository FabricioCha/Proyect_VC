import { useEffect, useState } from 'react';
import api from '../api/axiosConfig';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const CatalogPage = () => {
    const [products, setProducts] = useState([]);
    const [kits, setKits] = useState([]);
    const [cart, setCart] = useState([]);
    const { logout } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        fetchCatalog();
    }, []);

    const fetchCatalog = async () => {
        try {
            const [prodRes, kitRes] = await Promise.all([
                api.get('/productos'),
                api.get('/kits')
            ]);
            setProducts(prodRes.data);
            setKits(kitRes.data);
        } catch (error) {
            console.error("Error loading catalog", error);
        }
    };

    const addToCart = (item, type) => {
        setCart(prev => {
            const existing = prev.find(i => i.id === item.id && i.type === type);
            if (existing) {
                return prev.map(i => i.id === item.id && i.type === type ? { ...i, quantity: i.quantity + 1 } : i);
            }
            return [...prev, { ...item, type, quantity: 1 }];
        });
    };

    const placeOrder = async () => {
        try {
            const items = cart.map(item => ({
                productId: item.type === 'product' ? item.id : null,
                kitId: item.type === 'kit' ? item.id : null,
                quantity: item.quantity
            }));

            const response = await api.post('/orders', { items });
            alert(`Orden creada con éxito! ID: ${response.data.orderId}`);
            setCart([]);
            fetchCatalog(); // Refresh stock
        } catch (error) {
            alert('Error al crear la orden: ' + (error.response?.data?.message || error.message));
            console.error(error);
        }
    };

    return (
        <div className="container mt-4">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h1>Catálogo Utiles Arequipa</h1>
                <div>
                    <button className="btn btn-outline-danger" onClick={logout}>Cerrar Sesión</button>
                </div>
            </div>

            <div className="row">
                <div className="col-md-8">
                    <h3>Kits Escolares</h3>
                    <div className="row mb-4">
                        {kits.map(kit => (
                            <div className="col-md-6 mb-3" key={kit.id}>
                                <div className="card h-100 border-primary">
                                    <div className="card-body">
                                        <h5 className="card-title">{kit.name}</h5>
                                        <p className="card-text">{kit.description}</p>
                                        <p className="fw-bold">Precio: S/ {kit.price}</p>
                                        <p className="text-success">Stock Virtual: {kit.virtualStock}</p>
                                        <button 
                                            className="btn btn-primary"
                                            onClick={() => addToCart(kit, 'kit')}
                                            disabled={kit.virtualStock === 0}
                                        >
                                            Agregar Kit
                                        </button>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>

                    <h3>Productos Individuales</h3>
                    <div className="row">
                        {products.map(prod => (
                            <div className="col-md-4 mb-3" key={prod.id}>
                                <div className="card h-100">
                                    <div className="card-body">
                                        <h5 className="card-title">{prod.name}</h5>
                                        <p className="fw-bold">Precio: S/ {prod.price}</p>
                                        <p className="text-muted">Stock: {prod.stock}</p>
                                        <button 
                                            className="btn btn-outline-primary"
                                            onClick={() => addToCart(prod, 'product')}
                                            disabled={prod.stock === 0}
                                        >
                                            Agregar
                                        </button>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                <div className="col-md-4">
                    <div className="card shadow sticky-top" style={{ top: '20px' }}>
                        <div className="card-header bg-success text-white">
                            <h4 className="mb-0">Carrito de Compras</h4>
                        </div>
                        <div className="card-body">
                            {cart.length === 0 ? (
                                <p>El carrito está vacío</p>
                            ) : (
                                <ul className="list-group mb-3">
                                    {cart.map((item, idx) => (
                                        <li className="list-group-item d-flex justify-content-between lh-sm" key={idx}>
                                            <div>
                                                <h6 className="my-0">{item.name}</h6>
                                                <small className="text-muted">Cantidad: {item.quantity}</small>
                                            </div>
                                            <span className="text-muted">S/ {item.price * item.quantity}</span>
                                        </li>
                                    ))}
                                    <li className="list-group-item d-flex justify-content-between">
                                        <span>Total (PEN)</span>
                                        <strong>S/ {cart.reduce((sum, i) => sum + i.price * i.quantity, 0).toFixed(2)}</strong>
                                    </li>
                                </ul>
                            )}
                            <button 
                                className="btn btn-success w-100" 
                                disabled={cart.length === 0}
                                onClick={placeOrder}
                            >
                                Confirmar Orden
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CatalogPage;
