using Implementation.Domain.Entities;
using Implementation.Domain.Enums;
using Implementation.Infrastructure.Persistence;

namespace Implementation.Infrastructure.Seed;

public static class SeedData
{
    public static void Seed(RestaurantDbContext context)
    {
        if (context.Menus.Any())
        {
            return;
        }

        var year = DateTime.UtcNow.Year;

        var tomato = new Ingredient("Tomato", isVegetarian: true, isAllergen: false);
        var mozzarella = new Ingredient("Mozzarella", isVegetarian: true, isAllergen: true);
        var basil = new Ingredient("Basil", isVegetarian: true, isAllergen: false);
        var beefPatty = new Ingredient("Beef Patty", isVegetarian: false, isAllergen: false);
        var cheddar = new Ingredient("Cheddar", isVegetarian: true, isAllergen: true);
        var bun = new Ingredient("Brioche Bun", isVegetarian: true, isAllergen: true);
        var asparagus = new Ingredient("Asparagus", isVegetarian: true, isAllergen: false);
        var salmon = new Ingredient("Salmon Fillet", isVegetarian: false, isAllergen: true);
        var lemonButter = new Ingredient("Lemon Butter", isVegetarian: true, isAllergen: false);
        var pumpkin = new Ingredient("Pumpkin", isVegetarian: true, isAllergen: false);
        var truffle = new Ingredient("Black Truffle", isVegetarian: true, isAllergen: false);
        var risottoRice = new Ingredient("Arborio Rice", isVegetarian: true, isAllergen: false);

        var mainMenu = new Menu("Main Menu", "Signature dishes available all year round.");
        var seasonalMenu = new Menu("Seasonal Menu", "Dishes offered only during their season.");

        var margherita = new StandardItem(
            "Margherita Pizza",
            "Classic pizza with tomato, mozzarella and fresh basil.",
            basePrice: 32.00m,
            calories: 850,
            isAvailable: true);
        margherita.AddIngredient(tomato);
        margherita.AddIngredient(mozzarella);
        margherita.AddIngredient(basil);

        var cheeseburger = new StandardItem(
            "Classic Cheeseburger",
            "Beef patty with cheddar in a brioche bun.",
            basePrice: 28.50m,
            calories: 720,
            isAvailable: true);
        cheeseburger.AddIngredient(beefPatty);
        cheeseburger.AddIngredient(cheddar);
        cheeseburger.AddIngredient(bun);

        var unavailablePasta = new StandardItem(
            "Carbonara Pasta",
            "Creamy pasta with pancetta.",
            basePrice: 36.00m,
            calories: 980,
            isAvailable: false);

        var springAsparagus = new SeasonalItem(
            "Spring Asparagus Tagliatelle",
            "Fresh tagliatelle with green asparagus.",
            basePrice: 39.00m,
            calories: 640,
            seasonStart: new DateTime(year, 3, 1),
            seasonEnd: new DateTime(year, 5, 31),
            seasonalPriceModifier: 1.05m);
        springAsparagus.AddIngredient(asparagus);
        springAsparagus.AddIngredient(lemonButter);

        var summerSalmon = new SeasonalItem(
            "Grilled Summer Salmon",
            "Char-grilled salmon fillet with lemon butter.",
            basePrice: 54.00m,
            calories: 640,
            seasonStart: new DateTime(year, 6, 1),
            seasonEnd: new DateTime(year, 8, 31),
            seasonalPriceModifier: 1.10m);
        summerSalmon.AddIngredient(salmon);
        summerSalmon.AddIngredient(lemonButter);

        var autumnPumpkinSoup = new SeasonalItem(
            "Autumn Pumpkin Soup",
            "Creamy roasted pumpkin soup.",
            basePrice: 22.00m,
            calories: 380,
            seasonStart: new DateTime(year, 9, 1),
            seasonEnd: new DateTime(year, 11, 30),
            seasonalPriceModifier: 0.95m);
        autumnPumpkinSoup.AddIngredient(pumpkin);

        var winterTruffleRisotto = new SeasonalItem(
            "Winter Truffle Risotto",
            "Creamy arborio risotto with black truffle.",
            basePrice: 49.00m,
            calories: 720,
            seasonStart: new DateTime(year, 12, 1),
            seasonEnd: new DateTime(year + 1, 2, 28),
            seasonalPriceModifier: 1.20m);
        winterTruffleRisotto.AddIngredient(risottoRice);
        winterTruffleRisotto.AddIngredient(truffle);

        mainMenu.AddItem(margherita);
        mainMenu.AddItem(cheeseburger);
        mainMenu.AddItem(unavailablePasta);

        seasonalMenu.AddItem(springAsparagus);
        seasonalMenu.AddItem(summerSalmon);
        seasonalMenu.AddItem(autumnPumpkinSoup);
        seasonalMenu.AddItem(winterTruffleRisotto);

var alice = new Customer(
            "Alice",
            "Smith",
            "alice.smith@example.com",
            GenderType.Female,
            memberSinceDate: new DateTime(2022, 3, 15),
            phoneNumber: "+48111222333");

        var bob = new Customer(
            "Bob",
            "Johnson",
            "bob.johnson@example.com",
            GenderType.Male,
            memberSinceDate: new DateTime(2023, 8, 1),
            phoneNumber: "+48555666777");

        var charlie = new Customer(
            "Charlie",
            "Brown",
            "charlie.brown@example.com",
            GenderType.Other,
            memberSinceDate: new DateTime(2024, 1, 20),
            phoneNumber: "+48555666888");

        var waiter = new Waiter(
            "Maria",
            "Garcia",
            "maria.garcia@example.com",
            GenderType.Female,
            RankType.Middle,
            employmentDate: new DateTime(2021, 5, 10),
            minimalSalary: 4200m,
            phoneNumber: "+48123456789");

        var cook = new Cook(
            "Jan",
            "Kowalski",
            "jan.kowalski@example.com",
            GenderType.Male,
            RankType.Senior,
            employmentDate: new DateTime(2018, 2, 1),
            minimalSalary: 5800m,
            phoneNumber: "+48123456788");

        var manager = new Manager(
            "Ewa",
            "Nowak",
            "ewa.nowak@example.com",
            GenderType.Female,
            RankType.Senior,
            employmentDate: new DateTime(2015, 9, 1),
            minimalSalary: 8500m,
            phoneNumber: "+48123456787");

var sampleOrder = alice.PlaceOrder();
        sampleOrder.AddItem(margherita, 2, margherita.GetCurrentPrice());
        sampleOrder.AddItem(cheeseburger, 1, cheeseburger.GetCurrentPrice());
        sampleOrder.RecordPayment("Card", "PAY-SAMPLE-001");
        sampleOrder.Accept();
        sampleOrder.ChangeStatus(StatusType.Preparing);
        sampleOrder.ChangeStatus(StatusType.Ready);
        sampleOrder.ChangeStatus(StatusType.Completed);

        context.Ingredients.AddRange(
            tomato, mozzarella, basil, beefPatty, cheddar, bun,
            asparagus, salmon, lemonButter, pumpkin, truffle, risottoRice);

        context.Menus.AddRange(mainMenu, seasonalMenu);
context.Customers.AddRange(alice, bob, charlie);
        context.Employees.AddRange(waiter, cook, manager);

        context.SaveChanges();
    }
}