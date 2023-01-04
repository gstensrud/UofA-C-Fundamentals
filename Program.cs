using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Linq.Expressions;
using System.Globalization;
using Xunit;


// declarations
bool debug = false;

const string RecipeList = "recipeList.txt";
const string RecipeDictionary = "recipeDictionary.txt";
string userChoice;
int selection;
bool goodUserChoice = false;
string servingSize;
string desiredServing;
string amount;
double newAmount;
string theIngredients;
string answer;
string ingredientUnit;
string adjustedAmount;
double finalAmount;
string recallRecipeList;
string recallRecipeDictionary;


List<string> newRecipe = new List<string>(); // create a list to store the recipe name, and the other things that I want to save to a file
List<string> ingredientName = new List<string>(); // Create a list to store the ingredient name
List<string> oldIngredientAmt = new List<string>();
List<string> newIngredientAmt = new List<string>();

List<string> unitsOfMeasure = new List<string>();
Dictionary<string, string> originalIngredients = new Dictionary<string, string>(); // Create a dictionary to store the Amount of ingredient (KEY) and the Unit of measure (VALUE)
Dictionary<string, string> newIngredients = new Dictionary<string, string>();




Console.WriteLine("     Welcome to my EXSM 3941 C# Fundamentals Assignment!!");
Console.WriteLine("\n\n");
Console.WriteLine("     Let's Resize a Recipe!!! (really, we're just doing some math :)\n");

//Menu 

while (!goodUserChoice)
{
MainMenu:
    Console.Write("             (1) Enter a recipe to Convert: \n               OR\n     " +
        "        (2) Recall a Saved Recipe\n               OR\n             (3) Exit\n\t" +
        "           Please choose: ");
    userChoice = Console.ReadLine().Trim();
    goodUserChoice = TestUserChoice(userChoice);
    if (!goodUserChoice)
    {
        
        Console.Clear();
        Console.WriteLine("\n\n");
        Console.WriteLine("     That is NOT a valid MENU CHOICE!! Please enter (1), (2), or (3): ");
        continue;
    }
    Console.Clear();
    
    Console.WriteLine("");
    Console.WriteLine("");
    Console.WriteLine("     Thank you!");
    selection = int.Parse(userChoice); //takes the user choice and turns it to an interger, and saves that as variable name "selection"

    switch (selection)
    {

        case 1:
            servingSize = "";
            desiredServing = "";
            theIngredients = "";
            amount = "";
            newRecipe.Clear();
            ingredientName.Clear();
            oldIngredientAmt.Clear();
            newIngredientAmt.Clear();
            unitsOfMeasure.Clear(); 
            originalIngredients.Clear();
            newIngredients.Clear();


            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("     Please enter the Name of your Recipe: ");
            newRecipe.Add(Console.ReadLine().Trim());

        OriginalServingSize: // at index 0 of new recipe list
            Console.WriteLine("     Please enter the Original Serving Size of the ORIGINAL Recipe: ");
            servingSize = Console.ReadLine().Trim();
            bool originalServing = TestServingSize(servingSize);
            if (!originalServing)
            {
                
                Console.WriteLine("     This MUST be a whole positive number");
                goto OriginalServingSize;
            }
            // int.Parse(servingSize);
            newRecipe.Add(servingSize.Trim()); // adding to the list named newRecipe
            
        DesiredServings:  // at index 1 of new recipe list
            Console.WriteLine("     Please enter the Desired Serving Size: ");
            desiredServing = Console.ReadLine().Trim();
            bool wantedServing = TestServingSize(desiredServing);
            if (!wantedServing)
            {
                
                Console.WriteLine("     This MUST be a whole positive number");
                goto DesiredServings;
            }
            // int.Parse(desiredServing);
            newRecipe.Add(desiredServing.Trim()); // adding to the list named newRecipe
           
        IngredientType:
            while (theIngredients.ToUpper() != "DONE")
            {
                Console.WriteLine("     Please enter an ingredient (enter DONE if there are " +
                "no more ingredients to add): ");
                theIngredients = Console.ReadLine().Trim().ToUpper();

                if (theIngredients == "")
                {
                    
                    Console.WriteLine("     Please enter an ingredient!!  You can't have a recipe with out ingredients!!  Dummy!");
                    goto IngredientType;
                }
                
                if (theIngredients == "DONE")
                {
                    continue;
                }
                ingredientName.Add(theIngredients);
            IngredientAmount:

                Console.WriteLine("     Please enter the AMOUNT of " + theIngredients + ", WITHOUT the unit of measure" +
                    " that the original recipe calls for: ");
                amount = Console.ReadLine().Trim();
                if (amount == "")
                {
                    
                    Console.WriteLine("     What are you making?? You need to have an amount of an ingredient...Double Dummy!!");
                    goto IngredientAmount;
                }
                bool NumberOnlyAmount = TestingAmount(amount);
                if (!NumberOnlyAmount)
                {
                    
                    Console.WriteLine("     You must have an amount of " + theIngredients + " to add!!");
                    goto IngredientAmount;
                }
                
                //need to do math 
                //convert all to strings to doubles to do the math
                newAmount = Convert.ToDouble(amount) / Convert.ToDouble(servingSize);
                finalAmount = newAmount * Convert.ToDouble(desiredServing);

                // then convert back to string to print to file
                if (debug == true)
                {
                    Console.WriteLine("this is the single serving size:\n" + "    " + newAmount + "\n   and this is the scaled amount:\n    " + finalAmount + "\n");
                }
            IngedientUnit:
                Console.WriteLine("     Please enter the unit of measure for the " + amount + " of " + theIngredients + " that the " +
                    "original recipe calls for: ");
                ingredientUnit = Console.ReadLine().Trim();
                bool goodUnit = TestUnit(ingredientUnit);
                if (!goodUnit)
                {
                    
                    Console.WriteLine(" that is not a good unit of measure!!");
                    goto IngedientUnit;
                }
                
                adjustedAmount = Convert.ToString(finalAmount); // convert (final amount) currently a <double> to a <string>, and call it "adjustedAmount"
                //adding to the lists
                oldIngredientAmt.Add(amount.Trim());// add the original ingredient amount to the list called "oldIngredientAmt"
                newIngredientAmt.Add(adjustedAmount.Trim());// add the scaled ingredient amount to the list called "newIngredientAmt"
                unitsOfMeasure.Add(ingredientUnit.Trim());//add the unit of measure to the list called "unitsOfMeasure"
                //adding to the dictionaries
                originalIngredients.Add(theIngredients.Trim(), amount.Trim());// add "theIngredients" , and the original recipe "amount" to the "originalIngredients" dictionary
                newIngredients.Add(theIngredients.Trim(), adjustedAmount.Trim()+"   "+ingredientUnit.Trim());// add "theIngredients" , and the original recipe "adjustedAmount" to the "newIngredients" dictionary
            }
            
            //this is calling from lists I created

            //printing to console rather than saving to file
            Console.WriteLine("\nThe name of the recipe we modified is: " + newRecipe[0]);
            Console.WriteLine("The original serving size was: " + newRecipe[1]);
            Console.WriteLine("We have modified it to serve " + newRecipe[2] + "people instead.\n");
            //calling from lists
            for (int i=0; i<ingredientName.Count; i++)
            {
                Console.WriteLine(ingredientName[i] + "  " + newIngredientAmt[i] + "   " + unitsOfMeasure[i]);
            }


            //calling from dictionary
            foreach (KeyValuePair<string, string> moddedRecipe in newIngredients) 
                {
                    Console.WriteLine(moddedRecipe.Key + "    " + moddedRecipe.Value);
                }


            //writing with the lists
            using (StreamWriter writer = File.AppendText(RecipeList + ".txt"))
            {
                writer.WriteLine("\nThe name of the recipe we modified is: " + newRecipe[0]);
                writer.WriteLine("The original serving size was: " + newRecipe[1]);
                writer.WriteLine("We have modified it to serve " + newRecipe[2] + "people instead.\n");

                for (int i = 0; i < ingredientName.Count; i++)
                {
                    Console.WriteLine(ingredientName[i] + "  " + newIngredientAmt[i] + "   " + unitsOfMeasure[i]);
                }



            }
            //wtiting with the dictionaries
            
            using (StreamWriter writer = File.AppendText(RecipeDictionary + ".txt"))
            {
                writer.WriteLine("\nThe name of the recipe we modified is: " + newRecipe[0]);
                writer.WriteLine("The original serving size was: " + newRecipe[1]);
                writer.WriteLine("We have modified it to serve " + newRecipe[2] + "people instead.\n");

                foreach (KeyValuePair<string, string> moddedRecipe in newIngredients)
                {
                    Console.WriteLine(moddedRecipe.Key + "    " + moddedRecipe.Value);
                }


            }
      
        AddRecipe:
            
                bool goodAnswer = false;
            
            while (goodAnswer == false)
                {
                    Console.WriteLine("     Thank you!!  You're recipe has been saved and converted from " + servingSize
                        + " to " + desiredServing + "'s.\nWould you like to enter another recipie to have converted" +
                        "to a different serving size? (Y or N): ");
                    answer = Console.ReadLine().ToUpper().Trim();
                    if (answer == "")
                    {
                        
                        Console.WriteLine("     Sorry, but that response is not valid");
                        goto AddRecipe;
                    }
                    else if (answer == "yes" || answer == "YES" || answer == "Yes" || answer == "Y" || answer == "y")
                    {
                        goodAnswer = true;
                    // Need to add all of the information to the dictionary!!
                        Console.WriteLine("     Thank you! Let's do it again!");
                        goto case 1;
                    }
                    else if (answer == "NO" || answer == "no" || answer == "No" || answer == "N" || answer == "n")
                    {
                        goodAnswer = true;
                        continue;
                    }
                }
            Console.Clear();
            Console.WriteLine("     Thank you!  Your recipe has been saved, and modified!");
         
            Console.WriteLine("Press 'M' go back to the main menu.");
            if (Console.ReadKey().Key == ConsoleKey.M)
            {
                goto MainMenu;
            }
            
            break;
        case 2:
            //These wont' work unless you change the file location to where thery are on the usere's PC

            //          recallRecipeList = File.ReadAllText(@"E:\U of A\C#\EXSM3941 C# Project\bin\Debug\net6.0\recipeList.txt");

            // Display the file contents to the console. Variable text is a string.
            //          Console.WriteLine("Recalled from the created lists");
            //          Console.WriteLine("Contents of WriteText.txt = {0}", recallRecipeList);
            //          Console.WriteLine("Press any key to exit.");
            //          Console.ReadKey();
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt")))
                

                    recallRecipeDictionary = File.ReadAllText(@"E:\U of A\C#\EXSM3941 C# Project\bin\Debug\net6.0\recipeDictionary.txt");

            // Display the file contents to the console. Variable text is a string.
            Console.WriteLine("Recalled from the created lists");
            Console.WriteLine("Contents of WriteText.txt = {0}", recallRecipeDictionary);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();


            break;
        case 3:
            break;
        default:
            break;
    }
}




bool TestUserChoice(string userChoiceTest)
{    
    return new Regex(@"^[1-3]$").IsMatch(userChoiceTest);
}

bool TestServingSize(string servingSizeTest)
{
    return new Regex(@"^[0-9]+$").IsMatch(servingSizeTest);
}

bool CheckDone(string theIngredientsTest)
{
    return new Regex(@"^DONE$").IsMatch(theIngredientsTest);
}

bool TestAnswer(string answerTest)
{
    return new Regex(@"[Y]").IsMatch(answerTest);
}

bool TestingAmount(string testAmount)
{
    return new Regex(@"^[.0-9]+$").IsMatch(testAmount);
}

bool TestUnit(string testingUnits)
{
    return new Regex(@"^[a-zA-Z.]+$").IsMatch(testingUnits);
}









public class Testing
{
    bool TestUserChoice(string userChoiceTest)
    {
        return new Regex(@"^[1-3]$").IsMatch(userChoiceTest);
    }
    [Theory,
        InlineData("1", true),
        InlineData("2", true),
        InlineData("3", true),
        InlineData("20",false),
        InlineData("0", false),
        InlineData(".", false),
        InlineData("a", false),
        InlineData("o", false),
        InlineData("ldfh", false),
        ]
    public void TestMenu(string toTest, bool expectedResult)
    {
        Assert.Equal(expectedResult, TestUserChoice(toTest));
    }
    bool TestServingSize(string servingSizeTest)
    {
        return new Regex(@"^[0-9]+$").IsMatch(servingSizeTest);
    }
    [Theory,
        InlineData("1", true),
        InlineData("2", true),
        InlineData("3", true),
        InlineData("4", true),
        InlineData("0", true),
        InlineData("adsf", false),
        InlineData("1.054654asdf", false),
        InlineData(".045", false),
        InlineData(".asdf", false),
        InlineData("-5", false),
        InlineData("25", true),
        ]
    public void TestingServingSize(string toTest, bool expectedResult)
    {
        Assert.Equal(expectedResult, TestServingSize(toTest));
    }
    bool TestCheckDone(string theIngredientsTest)
    {
        return new Regex(@"^DONE$").IsMatch(theIngredientsTest);
    }
    [Theory,
        InlineData("done", false),
        InlineData("anything", false),
        InlineData("DONE", true),
        ]
    public void TestingCheckDone(string toTest, bool expectedResult)
    {
        Assert.Equal(expectedResult, TestCheckDone(toTest));
    }
    bool TestAnswer(string answerTest)
    {
        return new Regex(@"[Y]").IsMatch(answerTest);
    }
    [Theory,
        InlineData("Y", true),
        InlineData("anything", false),
        InlineData("done", false),
        InlineData("y", false),
        InlineData("n", false),
        InlineData("N", false),
        InlineData("4565", false),
        ]
    public void TestingAnswer(string toTest, bool expectedResult)
    {
        Assert.Equal(expectedResult, TestAnswer(toTest));
    }


    bool TestingAmount(string testAmount)
    {
        return new Regex(@"^[.0-9]+$").IsMatch(testAmount);
    }
    [Theory,
        InlineData("0.5", true),
        InlineData("anything", false),
        InlineData("-6", false),
        InlineData("y", false),
        InlineData("n", false),
        InlineData("N", false),
        InlineData("45.65", true),
        InlineData("4565", true),
        InlineData("45 65", false),
        ]
    public void TestAmount(string toTest, bool expectedResult)
    {
        Assert.Equal(expectedResult, TestingAmount(toTest));
    }

    bool TestingUnit(string testingUnits)
    {
        return new Regex(@"^[a-zA-Z.]+$").IsMatch(testingUnits);
    }
    [Theory,
        InlineData("0", false),
        InlineData("g", true),
        InlineData("gg", true),
        InlineData("tbsp.", true)
        ]
    public void TestUnit(string toTest, bool expectedResult)
    {
        Assert.Equal(expectedResult, TestingUnit(toTest));
    }


}

