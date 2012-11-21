using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OldFrenchayInn;

namespace OFITests {
    [TestClass]
    public class SystemTesting {

        [TestMethod]
        public void CardDetailsCreationTest(){
            string number = "2664311";
            string issueDate = "10/13";
            string expiryDate = "10/15";

            CardDetails details = new CardDetails(number, issueDate, expiryDate);

            Assert.AreEqual(details.Number, number);
            Assert.AreEqual(details.IssueDate, issueDate);
            Assert.AreEqual(details.ExpiryDate, expiryDate);

        }

        [TestMethod]
        public void TestBookingCreation(){
            CardDetails details = new CardDetails("01912664311", "10/11", "10/13");
            Customer customer = new Customer("Customer", "Customer@Customer.internet", details);
            Room room = Room.Get(1);
            DateTime start = new DateTime(2012, 4, 1);
            DateTime end = new DateTime(2012, 4, 5);
            Booking booking = new Booking(start, end, customer, room);

            Assert.AreEqual(string.Format("Room {0} for {1} from {2} to {3}", room.Number, customer, start.ToShortDateString(),
                                 end.ToShortDateString()), booking.ToString());

        }

        [TestMethod]
        public void RoomCreationTest(){
            for (int i = 1; i < 8; i++){
                var room = Room.Get(i);
            }
            try{
                Room.Get(0);
                Assert.Fail("Allowed out of bounds");
            }
            catch(IndexOutOfRangeException){
                //Swallow
            }

            try{
                Room.Get(9);
                Assert.Fail("Allowed too large room number");
            }
            catch(IndexOutOfRangeException){
                //Swallow
            }
        }

        [TestMethod]
        public void CustomerCreationTest(){
            CardDetails details = new CardDetails("01912664311", "10/11", "10/13");
            Customer customer = new Customer("Customer", "Customer@Customer.internet", details);

            Assert.AreEqual("Customer (Customer@Customer.internet)", customer.ToString());
        }

        [TestMethod]
        public void RoomFeatureCreationTest(){
            string description = "This stunning addition to any room will leave you smokin'!";
            RoomFeature feature = new RoomFeature("Lasers", description);
            Assert.AreEqual("Lasers", feature.ToString());
            Assert.AreEqual(description, feature.Description);

            var minibar = RoomFeature.Get(RoomFeatures.Minibar);
            var TV = RoomFeature.Get(RoomFeatures.TV);
            var internet = RoomFeature.Get(RoomFeatures.Internet);
        }

        [TestMethod]
        public void TestBookingCollisions(){
            CardDetails details = new CardDetails("01912664311", "10/11", "10/13");
            Customer customer = new Customer("Customer", "Customer@Customer.internet", details);
            Room room = Room.Get(1);

            Booking booking1 = new Booking(new DateTime(2012, 4, 1), new DateTime(2012, 4, 5), customer, room);
            Booking bookingConflict = new Booking(new DateTime(2012, 4, 3), new DateTime(2012, 4, 6), customer, room);
            Booking bookingNoConflict = new Booking(new DateTime(2012, 4, 9), new DateTime(2012, 4, 12), customer, room);
            Booking bookingDifferentRoom = new Booking(new DateTime(2012, 4, 3), new DateTime(2012, 4, 6), customer,
                                                       Room.Get(2));
            Booking bookingDifferentRoomNoConflict = new Booking(new DateTime(2012, 4, 12), new DateTime(2012, 4, 19),
                                                                 customer, Room.Get(2));

            Assert.IsTrue(booking1.Contains(bookingConflict));
            Assert.IsTrue(bookingConflict.Contains(booking1));
            Assert.IsFalse(booking1.Contains(bookingNoConflict));
            Assert.IsFalse(booking1.Contains(bookingDifferentRoom));
            Assert.IsFalse(booking1.Contains(bookingDifferentRoomNoConflict));

        }

        [TestMethod]
        public void TestListCollapsing(){
            var testList = new List<string>{"1", "2", "3", "5", "7", "8", "9", "12", "15", "16", "17"};
            var newList = new List<string>(testList.CollapseRanges());
            var result = string.Join(", ", newList);
            Assert.AreEqual("1-3, 5, 7-9, 12, 15-17", result);
        }
    }
}
