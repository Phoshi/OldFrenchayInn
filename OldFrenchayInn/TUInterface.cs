using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace OldFrenchayInn {
    /// <summary>
    /// Text User Interface class
    /// </summary>
    class TUInterface{
        /// <summary>
        /// How many days the month of april contains
        /// </summary>
        private const int DAYS_IN_APRIL = 30;

        /// <summary>
        /// Outputs a message to the user, with or without a terminator.
        /// Terminator is newline on this system.
        /// </summary>
        /// <param name="output">Message to show the user</param>
        /// <param name="terminate">Whether to terminate message or allow subsequent messages to continue this one</param>
        private static void Out(object output, bool terminate = true){
            if (terminate){
                Console.WriteLine(output);
            }
            else{
                Console.Write(output);
            }
        }

        /// <summary>
        /// Asks a yes|no question of the user, returns whether they said yes.
        /// </summary>
        /// <param name="question">The question to be asked</param>
        /// <returns>The user's response</returns>
        private static bool Ask(string question){
            Out(question, false);
            var result = In(new[]{"y", "n"},
                            validationFunction:
                                (iter, item) => iter.Contains(item.First().ToString(CultureInfo.InvariantCulture)));
            return result.StartsWith("y");
        }

        /// <summary>
        /// Takes in input from the user, optionally offering them only a selection of options.
        /// Will collapse ranges of numbers in all-numeric option sets. e.g. [1, 2, 3, 5] => [1-3, 5]
        /// Will continue to ask for new input until the validation is fulfilled.
        /// </summary>
        /// <param name="options">Possible options the user can select. If all numeric, range collapsing will occur.</param>
        /// <param name="validationFunction">An optional function to validate with. Must be of the signature (IEnumerable<string>, string) : bool. Will be IEnumerable.Contains by default.</string></param>
        /// <returns>The user's valid inputted string.</returns>
        private static string In(IEnumerable<string> options = null, Func<IEnumerable<string>, string, bool> validationFunction = null){
            if (options == null && validationFunction==null){
                return Console.ReadLine();
            }

            if (validationFunction==null){
                validationFunction = (iterable, current) => iterable.Contains(current);
            }

            if (options != null){
                Out(string.Format("[{0}] ", string.Join(", ", options.CollapseRanges())), false);
            }
            string answer = Console.ReadLine();

            if (validationFunction(options, answer)){
                return answer;
            }

            Out(answer + " is not a possible input! Please try again:");
            return In(options);
        }

        /// <summary>
        /// Main loop method
        /// </summary>
        /// <returns>True for continue looping, false otherwise.</returns>
        public bool Loop(){
            //Begin

            Out("Welcome to Old Frenchay Inn!");
            var seeRooms = Ask("Would you like to see our rooms?");
            if (!seeRooms){
                Out("Okay then.");
                return true;
            }

            PrintRoomTypeList();

            //Selecting room type

            Out("Which room type would you like to book?");
            var type = GetRoomTypeFromUser();

            //Selecting date

            Out("Sweet. When from? We only have april.");
            var chosenStartDate = GetStartDateFromUser();


            Out("Okay. How long for? You can't leave april. Sorry.");
            var chosenNumberOfDays = GetNumberOfDaysFromUser(chosenStartDate);

            var booking = MakeBooking(chosenStartDate, chosenNumberOfDays, type);
            if (booking == null){
                Out("Oh! Sorry, we don't have that time free.");
                return true;
            }

            //Accepting price

            double price = booking.GetPrice();
            Out(string.Format("Okay, that'll be £{0}.", price));

            if (!Ask("That okay?")){
                Out("Okay. There's a cardboard box just down the road.");
                return true;
            }

            //Taking name

            Out("Sweet. I'm just gonna need your name: ", false);
            string name = In();
            Out("Your email address: ", false);
            string email = In();

            //Finishing booking

            Out("Thanks. Gimme a sec to make your booking", false);
            DoLongWork();

            Booking.AddBooking(booking);
            Out(string.Format("Okay, your booking number is {0}.", booking.ReservationNumber));

            //Taking card details

            Out("Let's just finalise this and we're all done, okay? Card number: ", false);
            var cardNumber = GetCardNumber();
            Out("And that was issued on:", false);
            string issueDate = In();
            Out("And expires on:", false);
            string expiryDate = In();
            Out("Gotcha. Let's just finalise this", false);
            DoLongWork();

            var details = new CardDetails(cardNumber, issueDate, expiryDate);
            var customer = new Customer(name, email, details);

            booking.Customer = customer;

            Out(string.Format("Alrighty, we're taking the payment now. I can confirm your booking is {0}.", booking));

            return true;
        }

        private static string GetCardNumber(){
            string cardNumber = In(validationFunction: (iter, item) => IsNumericValidator.IsNumeric(item));
            return cardNumber;
        }

        private static Booking MakeBooking(int chosenStartDate, int chosenNumberOfDays, RoomType type){
            Booking booking;
            try{
                booking = Booking.Create(new DateTime(2013, 4, chosenStartDate), chosenNumberOfDays, type);
            }
            catch (OutOfRoomException){
                return null;
            }
            return booking;
        }

        private static int GetNumberOfDaysFromUser(int chosenStartDate){
            int chosenNumberOfDays = int.Parse(
                In(from number in Enumerable.Range(1, DAYS_IN_APRIL - chosenStartDate)
                   select number.ToString(CultureInfo.InvariantCulture))
                );
            return chosenNumberOfDays;
        }

        private static int GetStartDateFromUser(){
            int chosenStartDate = int.Parse(
                In(from number in Enumerable.Range(1, DAYS_IN_APRIL) select number.ToString(CultureInfo.InvariantCulture))
                );
            return chosenStartDate;
        }

        private static RoomType GetRoomTypeFromUser(){
            var chosenType = In(new[]{"Single", "Double", "Family"},
                                validationFunction: (iter, item) => iter.ToLower().Contains(item.ToLower()));
            RoomTypes types;
            bool success = RoomTypes.TryParse(chosenType, ignoreCase: true, result: out types);
            if (!success){
                Out("General Failure.");
                return null;
            }
            RoomType type = RoomType.Get(types);
            return type;
        }

        private static void PrintRoomTypeList(){
            foreach (RoomTypes roomType in new[]{RoomTypes.Single, RoomTypes.Double, RoomTypes.Family}){
                Out(RoomType.Get(roomType));
            }
        }

        private static void DoLongWork(){
            for (var i = 0; i < 3; i++) {
                Thread.Sleep(1000);
                Out(".", false);
            }
            Out("!");
        }
    }
}
