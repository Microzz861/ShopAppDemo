using ShopLibrary.Interfaces;
using ShopLibrary.Services;
using ShopLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using System.Xml.Linq;

namespace ShopApp
{
    public class UserInterface
    {
        public ICustomerService _customerService { get; set; }
        public IItemService _itemService { get; set; }
        public ISupplierService _supplierService { get; set; }

        public UserInterface(ICustomerService customerService, IItemService itemService, ISupplierService supplierService) {
            _customerService = customerService;
            _itemService = itemService;
            _supplierService = supplierService;
        }

        public async Task supplierNameInterface()
        {
            Console.WriteLine("What is your name?");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";
            var supplier = await _supplierService.GetSupplierByNameAsync(name);
            if (supplier != null)
            {
                Console.WriteLine();
                Console.WriteLine($"Your name is: {supplier.Name} and your current profit is: {supplier.Profit}\n");
                await supplierHomeInterface(supplier);
            }
            else
            {
                Console.WriteLine($"\n{name} was not found in the database");
                Console.WriteLine("\nPerhaps you misspelt your name?");
                Console.WriteLine("Would you like to reenter your name?\n");
                Console.WriteLine("1) Yes");
                Console.WriteLine("2) No (Exit the app)\n");
                string choiceInput = Console.ReadLine() ?? string.Empty;

                if (choiceInput != null)
                {
                    int choice;
                    if (int.TryParse(choiceInput, out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                await supplierNameInterface();
                                break;
                            case 2:
                                Console.WriteLine("Bye then!");
                                Environment.Exit(0);
                                break;

                        }
                    }

                }
            }
        }
        public async Task supplierHomeInterface(Supplier supplier)
        {
            string name = supplier.Name;
            Console.WriteLine($"Welcome {name} to the Shop, what would you like to do today?");
            Console.WriteLine("1) See all of your items?");
            Console.WriteLine("2) Remove yourself from the shops database");
            string choiceInput = Console.ReadLine() ?? string.Empty;
            Console.WriteLine();
            if (choiceInput != null)
            {
                int choice;
                if (int.TryParse(choiceInput, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            var itemList = supplier.items;
                            var groupedItems = from item in itemList group item by item.Category;
                            foreach (var group in groupedItems)
                            {
                                Console.WriteLine(group.Key.ToUpper());
                                foreach (var item in group)
                                {
                                    Console.WriteLine($"Item: {item.Name}, Price: {item.Price}, Count: {item.Count}");
                                }
                                Console.WriteLine();

                            }
                            break;
                        case 2:
                            
                            await _supplierService.DeleteSupplierByIdAsync(supplier.Id);
                            Console.WriteLine($"Successfully removed {name} from the database");
                            Console.WriteLine("Goodbye User");
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Choice Selected");
                    Console.WriteLine();
                    await supplierHomeInterface(supplier);
                }
            }

        }

        public async Task customerNameInterface()
        {
            Console.WriteLine("What is your name?");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";
            var customer = await _customerService.GetCustomerByNameAsync(name);
            if (customer != null)
            {
                Console.WriteLine();
                Console.WriteLine($"Your name is: {customer.Name} and your budget is: {customer.Budget}\n");
                await customerHomeInterface(customer.Name);
            }
            else
            {
                Console.WriteLine($"\n{name} was not found in the database");
                Console.WriteLine("Would you like to become a new customer with a budget of 50$? yes/no\n");
                string input = Console.ReadLine() ?? string.Empty;
                if (input != null)
                {
                    if (input == "yes")
                    {
                        await _customerService.AddCustomerAsync(name);
                        Console.WriteLine("Now you must reenter your name!");
                        await customerNameInterface();
                    }
                    else
                    {

                        Console.WriteLine("\nPerhaps you misspelt your name?");
                        Console.WriteLine("Would you like to reenter your name?\n");
                        Console.WriteLine("1) Yes");
                        Console.WriteLine("2) No (Exit the app)\n");
                        string choiceInput = Console.ReadLine() ?? string.Empty;

                        if (choiceInput != null)
                        {
                            int choice;
                            if (int.TryParse(choiceInput, out choice))
                            {
                                switch (choice)
                                {
                                    case 1:
                                        await customerNameInterface();
                                        break;
                                    case 2:
                                        Console.WriteLine("Bye then!");
                                        Environment.Exit(0);
                                        break;

                                }
                            }

                        }
                    }
                }
            }
        }

        public async Task itemNameInterface(List<Item> itemList, string identifier,string type)//Change to DTO
        {
            Console.WriteLine("Please insert the name of the item you would like to purchase");
            string itemName = Console.ReadLine() ?? string.Empty;
            if (itemName != string.Empty)
            {
                var item = (from items in itemList where items.Name == itemName select items).SingleOrDefault();
                if (item != null)
                {
                    Console.WriteLine();
                    if (type == "Customer")
                    {
                        await itemPurchaseInterface(item, identifier, "Customer");
                    }
                    else if(type == "Manager")
                    {
                        await itemPurchaseInterface(item, identifier, "Manager");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid Item name");
                    Console.WriteLine();
                    await itemNameInterface(itemList, identifier, type);
                }

            }
        }

        public async Task itemPurchaseInterface(Item item, string identifier,string type)
        {
            await Console.Out.WriteLineAsync($"Name: {item.Name}, Price: {item.Price}, Count: {item.Count}");
            Console.WriteLine($"Please enter the amount of {item.Name} you want to purchase.");
            await Console.Out.WriteLineAsync($"You may purchase up to {item.Count} {item.Name}.");

            string quantityInput = "";
            int quantity = 0; 
            while(quantity > item.Count || quantity < 0 || quantityInput == string.Empty)
            {
                    quantityInput = Console.ReadLine() ?? string.Empty;
                    Console.WriteLine();
                    if (int.TryParse(quantityInput, out quantity))
                    {
                    if (type == "Customer")
                    {
                        double profit = await _customerService.PurchaseItemAsync(identifier, item.Name, quantity);
                        Console.WriteLine($"Thank you for your purchase, we have gained {profit}");
                    }
                    else
                    {
                        double profit = await _itemService.BuyFromSupplierAsync(identifier, item, quantity);
                        Console.WriteLine($"Thank you for your purchase, we have gained {quantity} {item.Name}/s");
                    }
                    }
                    else
                    {
                        Console.WriteLine("Invalid amount entered. Please enter the amount again");
                    }


            }
        }
        public async Task customerHomeInterface(string name)
        {
            Console.WriteLine($"Welcome {name} to the Shop, what would you like to do today?");
            Console.WriteLine("1) Purchase Items");
            Console.WriteLine("2) Remove yourself from the shops database");
            string choiceInput = Console.ReadLine() ?? string.Empty;
            Console.WriteLine();
            if(choiceInput != null)
            {
                int choice;
                if (int.TryParse(choiceInput, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            var itemList = await _itemService.GetAllItemsAsync();
                            var groupedItems = from item in itemList group item by item.Category;
                            foreach (var group in groupedItems)
                            {
                                Console.WriteLine(group.Key.ToUpper());
                                foreach (var item in group)
                                {
                                    Console.WriteLine($"Item: {item.Name}, Price: {item.Price}, Count: {item.Count}");
                                }
                                Console.WriteLine();        
                                
                            }
                            await itemNameInterface(itemList, name, "Customer");
                            break;
                        case 2:
                            await _customerService.DeleteCustomerByNameAsync(name);
                            Console.WriteLine($"Successfully removed {name} from the database");
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Choice Selected");
                    Console.WriteLine();
                    await customerHomeInterface(name);
                }
            }

        }

        public async Task managerHomeInterface()
        {
            Console.WriteLine($"Welcome Manager to the Shop, what would you like to do today?");
            Console.WriteLine("1) See all of the shops items?");
            Console.WriteLine("2) Check the stores total profit?");
            Console.WriteLine("3) Restock items from suppliers?");
            string choiceInput = Console.ReadLine() ?? string.Empty;
            Console.WriteLine();
            if (choiceInput != null)
            {
                int choice;
                if (int.TryParse(choiceInput, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            {
                                var itemList = await _itemService.GetAllItemsAsync();
                                var groupedItems = from item in itemList group item by item.Category;
                                foreach (var group in groupedItems)
                                {
                                    Console.WriteLine(group.Key.ToUpper());
                                    foreach (var item in group)
                                    {
                                        Console.WriteLine($"Item: {item.Name}, Price: {item.Price}, Count: {item.Count}");
                                    }
                                    Console.WriteLine();

                                }
                                break;
                            }
                        case 2:
                            {

                                var itemList = await _itemService.GetAllItemsAsync();
                                var groupedItems = from item in itemList group item by item.Category;
                                var sum = itemList.Sum(item =>  item.Profit);
                                foreach (var group in groupedItems)
                                {
                                    Console.WriteLine(group.Key.ToUpper());
                                    foreach (var item in group)
                                    {
                                        Console.WriteLine($"Item: {item.Name}, Profit: {item.Profit}");
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine($"Total Profit : {sum}");
                                break;
                            }
                        case 3:
                            {
                                await itemSuppliersInterface();
                                break;
                            }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Choice Selected");
                    Console.WriteLine();
                    await managerHomeInterface();
                }
            }

        }


        public async Task itemSuppliersInterface()
        {
            List<Supplier> supplierList = await _supplierService.GetAllSuppliersAsync();
            var suppliers = (from supplier in supplierList select new { name = supplier.Name, items = supplier.items.Select(item => item.Name)}).ToList();
            
            foreach ( var supplier in suppliers)
            {
                Console.WriteLine(supplier.name);
                Console.WriteLine("Retail Items: " + String.Join(", ", supplier.items));
                Console.WriteLine();
            }
            await itemSupplierNameInterface(supplierList);
           
        }

        public async Task itemSupplierNameInterface(List<Supplier> supplierList)
        {
                Console.WriteLine("Please insert the name of the item you would like to purchase");
                string supplierName = Console.ReadLine() ?? string.Empty;
                if (supplierName != string.Empty)
                {
                    var supplier = (from suppliers in supplierList where suppliers.Name == supplierName select suppliers).SingleOrDefault();
                    if (supplier != null)
                    {
                        Console.WriteLine();
                        await supplierItemPurchaseInterface(supplier);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid supplier name");
                        Console.WriteLine();
                        await itemSupplierNameInterface(supplierList);
                    }

                }
            
        }

        public async Task supplierItemPurchaseInterface(Supplier supplier)
        {
            Console.WriteLine($"What would you like to restock from {supplier.Name}?");
            var itemList = supplier.items;
            var groupedItems = from item in itemList group item by item.Category;
            foreach (var group in groupedItems)
            {
                Console.WriteLine(group.Key.ToUpper());
                foreach (var item in group)
                {
                    Console.WriteLine($"Item: {item.Name}, Price: {item.Price}, Count: {item.Count}");
                }
               
                
            }
            Console.WriteLine();
            await itemNameInterface(itemList, supplier.Id, "Manager");
        }
    }
}
