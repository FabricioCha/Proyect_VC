'use client';

import { useState, useMemo } from 'react';
import { useQuery, useMutation } from '@tanstack/react-query';
import api from '@/services/api';
import { useForm, useFieldArray } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Plus, Trash2, Check, ChevronsUpDown } from 'lucide-react';
import { useRouter } from 'next/navigation';

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "@/components/ui/command";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { cn } from "@/lib/utils";

// Types
type Product = {
    id: number;
    nombre: string;
    precio: number;
    stock: number;
    sku: string;
};

const kitSchema = z.object({
    nombre: z.string().min(1, "El nombre del kit es requerido"),
    items: z.array(z.object({
        productoId: z.number().min(1, "Selecciona un producto"),
        cantidad: z.coerce.number().min(1, "La cantidad debe ser al menos 1"),
    })).min(1, "Agrega al menos un producto al kit"),
});

type KitFormValues = z.infer<typeof kitSchema>;

export default function CreateKitPage() {
    const router = useRouter();
    
    // Fetch Products
    const { data: products } = useQuery<Product[]>({
        queryKey: ['products'],
        queryFn: async () => {
            const response = await api.get('/productos');
            return response.data;
        },
    });

    // Mutation
    const createKitMutation = useMutation({
        mutationFn: async (data: KitFormValues) => {
            await api.post('/kits', data);
        },
        onSuccess: () => {
            router.push('/admin'); // Redirect to dashboard
        },
    });

    // Form
    const form = useForm<KitFormValues>({
        resolver: zodResolver(kitSchema) as any,
        defaultValues: {
            nombre: '',
            items: [{ productoId: 0, cantidad: 1 }],
        },
    });

    const { fields, append, remove } = useFieldArray({
        control: form.control,
        name: "items",
    });

    // Calculate Total Price
    const watchedItems = form.watch("items");
    const totalPrice = useMemo(() => {
        if (!products) return 0;
        return watchedItems.reduce((total, item) => {
            const product = products.find(p => p.id === item.productoId);
            return total + (product ? product.precio * item.cantidad : 0);
        }, 0);
    }, [watchedItems, products]);

    const onSubmit = (data: KitFormValues) => {
        createKitMutation.mutate(data);
    };

    return (
        <div className="max-w-4xl mx-auto">
            <h1 className="text-3xl font-bold mb-6">Crear Nuevo Kit Escolar</h1>
            
            <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
                    <Card>
                        <CardHeader>
                            <CardTitle>Información General</CardTitle>
                        </CardHeader>
                        <CardContent>
                            <FormField
                                control={form.control}
                                name="nombre"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Nombre del Kit</FormLabel>
                                        <FormControl>
                                            <Input placeholder="Ej. Kit Primaria 2025" {...field} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </CardContent>
                    </Card>

                    <Card>
                        <CardHeader className="flex flex-row items-center justify-between">
                            <CardTitle>Productos del Kit</CardTitle>
                            <Button 
                                type="button" 
                                variant="outline" 
                                size="sm" 
                                onClick={() => append({ productoId: 0, cantidad: 1 })}
                            >
                                <Plus className="w-4 h-4 mr-2" /> Agregar Producto
                            </Button>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            {fields.map((field, index) => (
                                <div key={field.id} className="flex items-end gap-4 border-b pb-4">
                                    <FormField
                                        control={form.control}
                                        name={`items.${index}.productoId`}
                                        render={({ field }) => (
                                            <FormItem className="flex-1 flex flex-col">
                                                <FormLabel>Producto</FormLabel>
                                                <Popover>
                                                    <PopoverTrigger asChild>
                                                        <FormControl>
                                                            <Button
                                                                variant="outline"
                                                                role="combobox"
                                                                className={cn(
                                                                    "w-full justify-between",
                                                                    !field.value && "text-muted-foreground"
                                                                )}
                                                            >
                                                                {field.value
                                                                    ? products?.find(
                                                                        (product) => product.id === field.value
                                                                    )?.nombre
                                                                    : "Seleccionar producto"}
                                                                <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                                                            </Button>
                                                        </FormControl>
                                                    </PopoverTrigger>
                                                    <PopoverContent className="w-[400px] p-0">
                                                        <Command>
                                                            <CommandInput placeholder="Buscar producto..." />
                                                            <CommandList>
                                                                <CommandEmpty>No se encontró el producto.</CommandEmpty>
                                                                <CommandGroup>
                                                                    {products?.map((product) => (
                                                                        <CommandItem
                                                                            value={product.nombre}
                                                                            key={product.id}
                                                                            onSelect={() => {
                                                                                form.setValue(`items.${index}.productoId`, product.id);
                                                                            }}
                                                                        >
                                                                            <Check
                                                                                className={cn(
                                                                                    "mr-2 h-4 w-4",
                                                                                    product.id === field.value
                                                                                        ? "opacity-100"
                                                                                        : "opacity-0"
                                                                                )}
                                                                            />
                                                                            {product.nombre} - S/ {product.precio}
                                                                        </CommandItem>
                                                                    ))}
                                                                </CommandGroup>
                                                            </CommandList>
                                                        </Command>
                                                    </PopoverContent>
                                                </Popover>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                    
                                    <FormField
                                        control={form.control}
                                        name={`items.${index}.cantidad`}
                                        render={({ field }) => (
                                            <FormItem className="w-32">
                                                <FormLabel>Cantidad</FormLabel>
                                                <FormControl>
                                                    <Input type="number" min="1" {...field} />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />

                                    <Button
                                        type="button"
                                        variant="ghost"
                                        size="icon"
                                        className="text-red-500"
                                        onClick={() => remove(index)}
                                    >
                                        <Trash2 className="w-4 h-4" />
                                    </Button>
                                </div>
                            ))}
                            {form.formState.errors.items && (
                                <p className="text-sm font-medium text-destructive">
                                    {form.formState.errors.items.message}
                                </p>
                            )}
                        </CardContent>
                    </Card>

                    <div className="flex items-center justify-between p-6 bg-white rounded-lg shadow-sm border">
                        <div>
                            <p className="text-sm text-gray-500">Precio Total Estimado</p>
                            <p className="text-3xl font-bold text-blue-600">S/ {totalPrice.toFixed(2)}</p>
                        </div>
                        <div className="flex gap-4">
                            <Button 
                                type="button" 
                                variant="outline"
                                onClick={() => router.push('/admin')}
                            >
                                Cancelar
                            </Button>
                            <Button type="submit" disabled={createKitMutation.isPending}>
                                {createKitMutation.isPending ? 'Guardando...' : 'Crear Kit'}
                            </Button>
                        </div>
                    </div>
                </form>
            </Form>
        </div>
    );
}
