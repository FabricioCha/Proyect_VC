#!/bin/bash

BASE_URL="http://localhost:5000/api"

echo "--- 1. Testing Seed ---"
curl -s -X POST "$BASE_URL/seed/init"
echo -e "\n"

echo "--- 2. Testing Login (Admin) ---"
LOGIN_RESPONSE=$(curl -s -X POST "$BASE_URL/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@utiles.com", "password": "Admin123!"}')
echo "Response: $LOGIN_RESPONSE"

TOKEN=$(echo $LOGIN_RESPONSE | grep -o '"token":"[^"]*' | cut -d'"' -f4)

if [ -z "$TOKEN" ]; then
    echo "Error: Could not get token"
    exit 1
fi
echo -e "\nToken received successfully."

echo "--- 3. Testing Get Products ---"
curl -s -X GET "$BASE_URL/productos" \
  -H "Authorization: Bearer $TOKEN" | head -c 200
echo -e "...\n"

echo "--- 4. Testing Create Product ---"
CREATE_PROD_RESPONSE=$(curl -s -X POST "$BASE_URL/productos" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "nombre": "Producto Test Auto",
    "precio": 25.50,
    "stock": 100,
    "sku": "TEST-AUTO-001",
    "categoria": "Pruebas"
  }')
echo "Response: $CREATE_PROD_RESPONSE"
echo -e "\n"

# Extract ID of created product (assuming response contains id, or we fetch it)
# For simplicity, we'll just fetch products again to see if it's there
echo "--- 5. Verifying Product Creation ---"
curl -s -X GET "$BASE_URL/productos" \
  -H "Authorization: Bearer $TOKEN" | grep "Producto Test Auto"
echo -e "\n(If you see the product name above, creation was successful)"

echo "--- 6. Testing Create Kit ---"
# We need a valid product ID. Let's assume ID 1 exists from seed.
CREATE_KIT_RESPONSE=$(curl -s -X POST "$BASE_URL/kits" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "nombre": "Kit Test Auto",
    "items": [
        { "productoId": 1, "cantidad": 2 }
    ]
  }')
echo "Response: $CREATE_KIT_RESPONSE"
echo -e "\n"

echo "--- 7. Testing Get Kits ---"
curl -s -X GET "$BASE_URL/kits" \
  -H "Authorization: Bearer $TOKEN"
echo -e "\n"
