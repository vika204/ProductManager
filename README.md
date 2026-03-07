# ProductManager

A .NET MAUI application for managing warehouses and products, built with C# using layered architecture, dependency injection, and Shell navigation.

---

## Project Structure

```text
ProductManager/
├── ProductManager/               # .NET MAUI application
│   ├── Pages/                    # XAML pages
│   ├── Tools/                    # value converters
│   ├── Resources/                # styles, colors, fonts, images
│   ├── App.xaml                  # application resources
│   ├── AppShell.xaml             # shell navigation
│   └── MauiProgram.cs            # dependency injection registration
├── ProductManager.Common/        # shared enums and helper methods
├── ProductManager.DBModels/      # storage models
├── ProductManager.UIModels/      # ui models
├── ProductManager.Services/      # fake storage and storage service
├── ProductManager.Console/       # console logic
├── ProductManager.ConsoleApp/    # console application entry point
└── ProductManager.slnx           # solution file
