# Aasha Gifts - Photo Customization & Gift Printing Shop

A complete Razor Pages app in ASP.NET Core (.NET 8) for a custom photo gifts printing shop.

## Features

- Product listing by categories (Albums, Banners, Photo Gifts)
- Order form per product (name, email, image upload)
- Stores orders in SQLite and photo files in `wwwroot/uploads`
- Admin page to view/download orders and mark as completed
- Razorpay integration placeholder
- Minimal Bootstrap styling

## Getting Started

1. **Restore & Build:**
    ```
    dotnet restore
    dotnet build
    ```

2. **Run EF Core Migrations:**
    ```
    dotnet tool install --global dotnet-ef
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

3. **Run the App:**
    ```
    dotnet run
    ```

4. Visit: https://localhost:5001

## Project Structure

- **Models**: `Product`, `Order`
- **Pages**: `Index` (products), `Order` (per-product form), `Admin`, `Thanks`
- **wwwroot/uploads**: Uploaded user images
- **Data/AppDbContext.cs**: EF Core context

## Notes

- Add product images to `wwwroot/images` with filenames matching seed data.
- Razorpay integration placeholder is present in the Order form.