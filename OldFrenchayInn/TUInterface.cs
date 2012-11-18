using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OldFrenchayInn {
    /// <summary>
    /// Text User Interface class
    /// </summary>
    class TUInterface{
        private const int DAYS_IN_APRIL = 30;
        private static void Out(object output, bool newLine = true){
            if (newLine){
                Console.WriteLine(output);
            }
            else{
                Console.Write(output);
            }
        }

        private static bool Ask(string question){
            Out(question + " [y, n] ", false);
            string read = Console.ReadLine();
            return read.StartsWith("y");
        }

        private static string In(IEnumerable<string> options = null){
            if (options == null){
                return Console.ReadLine();
            }

            Out(string.Format("[{0}] ", string.Join(", ", options)), false);
            string answer = Console.ReadLine();

            if (options.Contains(answer)){
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
            Out("Welcome to Old Frenchay Inn!");
            bool seeRooms = Ask("Would you like to see our rooms?");
            if (!seeRooms){
                Out("Okay then.");
                return true;
            }

            foreach (RoomTypes roomType in new[]{RoomTypes.Single, RoomTypes.Double, RoomTypes.Family}){
                Out(RoomType.Get(roomType));
            }

            Out("Which room type would you like to book?");
            string chosenType = In(new[]{"Single", "Double", "Family"});
            RoomTypes types;
            bool success = RoomTypes.TryParse(chosenType, out types);
            if (!success){
                Out("General Failure.");
                return true;
            }
            RoomType type = RoomType.Get(types);

            Out("Sweet. When from? We only have april.");
            int chosenStartDate = int.Parse(
                In(from number in Enumerable.Range(1, DAYS_IN_APRIL) select number.ToString())
                );


            Out("Okay. How long for? You can't leave april. Sorry.");
            int chosenNumberOfDays = int.Parse(
                In(from number in Enumerable.Range(1, DAYS_IN_APRIL - chosenStartDate) select number.ToString())
                );

            Booking booking;
            try {
                booking = Booking.Create(new DateTime(2013, 4, chosenStartDate), chosenNumberOfDays, type);
            }
            catch (OutOfMemoryException) {
                Out("Oh! Sorry, we don't have that time free.");
                return true;
            }
            double price = booking.GetPrice();
            Out(string.Format("Okay, that'll be £{0}.", price));

            if (!Ask("That okay?")){
                Out("Okay. There's a cardboard box just down the road.");
                return true;
            }

            Out("Sweet. I'm just gonna need your name: ", false);
            string name = In();
            Out("Your email address: ", false);
            string email = In();

            Out("Thanks. Gimme a sec to make your booking", false);
            DoLongWork();

            Booking.AddBooking(booking);
            Out(string.Format("Okay, your booking number is {0}.", booking.ReservationNumber));
            Out("Let's just finalise this and we're all done, okay? Card number: ", false);
            string cardNumber = In();
            Out("And that was issued on:", false);
            string issueDate = In();
            Out("And expires on:", false);
            string expiryDate = In();
            Out("Gotcha. Let's just finalise this", false);
            DoLongWork();

            CardDetails details = new CardDetails(cardNumber, issueDate, expiryDate);
            Customer customer = new Customer(name, email, details);

            booking.Customer = customer;

            Out(string.Format("Alrighty, we're taking the payment now. I can confirm your booking is {0}.", booking));

            return true;
        }

        private void DoLongWork(){
            for (int i = 0; i < 3; i++) {
                Thread.Sleep(1000);
                Out(".", false);
            }
            Out("!");
        }
    }
}
