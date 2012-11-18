using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldFrenchayInn {
    public class Booking {
        private static List<Booking> _bookings = new List<Booking>();
        public static bool IsBooked(Booking booking){
            return _bookings.Any(existingBooking => existingBooking.Contains(booking));
        }
        public static void AddBooking(Booking booking){
            if (!IsBooked(booking)){
                _bookings.Add(booking);
            }
            else{
                throw new InvalidOperationException("Booking already exists!");
            }
        }

        public static Booking Create(DateTime start, int duration, RoomType type){
            DateTime end = start.AddDays(duration);
            IEnumerable<Room> eligableRooms = from room in Room.Rooms where room.Type.Equals(type) select room;

            foreach (Room eligableRoom in eligableRooms){
                Booking preliminaryBooking = new Booking(start, end, null, eligableRoom);
                if (!IsBooked(preliminaryBooking)){
                    return preliminaryBooking;
                }
            }
            throw new OutOfMemoryException("No more rooms!");
        }

        public DateTime StartDate { get; internal set; }
        public DateTime EndDate { get; internal set; }

        public Customer Customer { get; set; }

        public Room Room { get; internal set; }

        public int ReservationNumber { get; internal set; }

        public int Duration {get { return (EndDate - StartDate).Days; }}

        public Booking(DateTime start, DateTime end, Customer customer, Room room){
            ReservationNumber = _bookings.Count + 1;
            StartDate = start;
            EndDate = end;
            Customer = customer;
            Room = room;
        }

        public bool Contains(Booking other){
            if (other.Room != Room){
                return false;
            }
            return other.StartDate >= StartDate && other.StartDate <= EndDate ||
                other.EndDate >= StartDate && other.EndDate <= EndDate;
        }

        public double GetPrice(){
            return Room.Type.Price*Duration;
        }

        public override string ToString(){
            return string.Format("Room {0} for {1} from {2} to {3}", Room, Customer, StartDate.ToShortDateString(),
                                 EndDate.ToShortDateString());
        }
    }
}
