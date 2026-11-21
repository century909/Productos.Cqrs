# 05 · Productos + CQRS (Clean Architecture, Minimal API)

Mismo esqueleto que `03-CleanArchitecture-CQRS`, pero el agregado es **Producto** (catalogo/inventario)
en lugar de `TarjetaCredito`. Sirve para comparar el mismo patron CQRS con otro dominio y para
reutilizar el template en un CRUD con reglas de negocio.

Mantiene intactas las piezas del template CQRS: **mediator propio** (sin MediatR), pipeline de
behaviors, **read store separado** del repositorio de escritura y endpoints que solo conocen `ISender`.

```
src/
  Productos.Cqrs.Domain          Agregado Producto + MovimientoStock (entidad hija) + IProductoRepository (solo escritura) + IUnitOfWork.
  Productos.Cqrs.Application
    Abstractions/Messaging       IRequest, ICommand, IQuery, handlers, IPipelineBehavior, ISender
    Messaging/Sender.cs          Despachador (~50 lineas) con pipeline de behaviors
    Behaviors/                   LoggingBehavior, ValidationBehavior (FluentValidation)
    Abstractions/Data/IProductosReadStore   Puerto del modelo de LECTURA (devuelve DTOs)
    Abstractions/IGeneradorSku   Puerto al servicio externo que asigna el SKU
    Productos/Commands/<Caso>/   <Caso>Command + <Caso>CommandValidator + <Caso>CommandHandler
    Productos/Queries/           <Caso>Query + Handler (van directo al read store)
  Productos.Cqrs.Infrastructure  EF Core InMemory: ProductoRepository (escritura, con tracking),
                                 ProductosReadStore (lectura, AsNoTracking + proyecciones) y GeneradorSkuSimulado.
  Productos.Cqrs.Api             Endpoints que solo conocen ISender.
```

## Modelo de dominio

`Producto` es un agregado rico: su estado solo cambia por metodos que protegen invariantes.

| Regla | Donde vive | HTTP |
|---|---|---|
| `Nombre` obligatorio (3..120), `Categoria` valida, `Precio > 0`, `StockInicial >= 0` | `CrearProductoCommandValidator` (formato) | 400 |
| `Precio` entre `100` y `100.000.000`, `StockInicial >= 0` | `Producto.Crear` | 422 |
| `AjustarStock` no deja stock negativo y no opera sobre descontinuados | `Producto.AjustarStock` | 422 (cuando se exponga) |
| Solo se descontinua un producto activo y con motivo | `Producto.Descontinuar` | 422 (cuando se exponga) |

`AjustarStock` y `Descontinuar` no tienen endpoint en este template: los usa el seed y quedan como
modelo para agregar los comandos `AjustarStock` y `DescontinuarProducto`.

## Flujo de un Command

`POST /api/productos` → el body es el `CrearProductoCommand` → `ISender.Send`
→ `LoggingBehavior` → `ValidationBehavior` (400 si falla) → `CrearProductoCommandHandler`
→ `IGeneradorSku` (SKU simulado) → `Producto.Crear(...)` (422 si viola regla)
→ `IProductoRepository.Agregar` → `IUnitOfWork.SaveChangesAsync`.

## Flujo de una Query

`GET /api/productos/{id}` → `ObtenerProductoQuery` → handler → `IProductosReadStore`
(proyeccion directa a `ProductoResponse`, `AsNoTracking`, sin cargar el agregado).

## Endpoints

| Metodo | Ruta | Mensaje | Resultado |
|---|---|---|---|
| GET | `/api/productos?categoria=Electronica` | `ListarProductosQuery` | 200 |
| GET | `/api/productos/{id}` | `ObtenerProductoQuery` | 200 / 404 |
| GET | `/api/productos/{id}/movimientos` | `ListarMovimientosQuery` | 200 / 404 |
| POST | `/api/productos` | `CrearProductoCommand` | 201 / 400 / 422 |

## Equivalencias con el ejemplo de tarjetas

| 03 · Tarjetas | 05 · Productos |
|---|---|
| `TarjetaCredito` | `Producto` |
| `Movimiento` (consumo/pago) | `MovimientoStock` (entrada/salida) |
| `EmitirTarjetaCommand` | `CrearProductoCommand` |
| `ObtenerTarjeta` / `ListarTarjetas` / `ListarMovimientos` | `ObtenerProducto` / `ListarProductos` / `ListarMovimientos` |
| `IGeneradorNumeroTarjeta` | `IGeneradorSku` |
| `ITarjetaRepository` | `IProductoRepository` |
| `ITarjetasReadStore` | `IProductosReadStore` |
| `TarjetasDbContext` | `ProductosDbContext` |

## Agregar un caso de uso

1. Crear carpeta `Productos/Commands/<Caso>` con Command, Validator y Handler (o una Query + Handler).
2. Mapear el endpoint con `sender.Send(...)`. El registro de handlers/validators es automatico.

## Cuando usarla

- Catalogos con lecturas y escrituras de cargas o modelos distintos, reglas de inventario y
  necesidad de pipeline transversal (auditoria, validacion, transacciones).
- **Costo**: mas archivos por caso de uso que Clean "simple"; para un CRUD plano es sobreingenieria.

## Ejecutar

```bash
dotnet run --project src/Productos.Cqrs.Api   # http://localhost:5105/scalar
```
