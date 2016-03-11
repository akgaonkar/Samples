using System;
using System.Linq;

namespace CoffeeDutyTest
{
    class Program
    {
        static void Main(string[] args)
        {

            int[][] employees = new int[][] {  new int[] { 1, 4 },
                                                new int[] { 2, 4 },
                                                new int[] { 3, 4 },
                                                new int[] { 4, 8 },
                                                new int[] { 5, 7 },
                                                new int[] { 6, 7 },
                                                new int[] { 7, 8 },
                                                new int[] { 8, 8 }
                                               };
            while (true)
            {
                int[] meetingAttendees = ReadUserInput();
                int juniorMostEmployeeInMeeting = CoffeeDuty(employees, meetingAttendees);
                DisplayResult(juniorMostEmployeeInMeeting);
                AskUserIfTheyWantToContinue();
            }

        }

        /// <summary>
        /// displays results
        /// </summary>
        /// <param name="juniorMostEmployeeInMeeting"></param>
        private static void DisplayResult(int juniorMostEmployeeInMeeting)
        {
            Console.WriteLine("Coffee will be brought by: " + juniorMostEmployeeInMeeting.ToString());
            Console.WriteLine();

        }

        /// <summary>
        /// Offer user option to continue or exit program
        /// </summary>
        private static void AskUserIfTheyWantToContinue()
        {
            Console.WriteLine("Close window to end program OR Press Enter to continue...");
            Console.ReadLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Reads user input 
        /// </summary>
        /// <returns>integer array</returns>
        private static int[] ReadUserInput()
        {
            Console.WriteLine("Please input the meeting attendees comma seperated");
            return Console.ReadLine().Split(',').Select(x => Convert.ToInt32(x)).ToArray();
        }

        /// <summary>
        /// Returns the junior most attendees amongst the passed meeting attendees 
        /// </summary>
        /// <param name="employees">Employee hierarchy collection</param>
        /// <param name="meetingAttendees">Meeting attendees</param>
        /// <returns>Junior most attendee</returns>
        private static int CoffeeDuty(int[][] employees, int[] meetingAttendees)
        {
            //If number of meetingAttendees is 1, then he will have to bring his own coffee :)
            if (meetingAttendees.Length == 1) return meetingAttendees[0];

            int junior = 0;
            for (int i = 0; i < meetingAttendees.Length; i++)
            {
                if (junior == 0)
                    junior = GetJunior(meetingAttendees[i], meetingAttendees[i + 1], employees);
                else if (meetingAttendees[i] != junior)
                    junior = GetJunior(meetingAttendees[i], junior, employees);
            }
            return junior;

        }

        /// <summary>
        /// Gets the junior amongst the two attendees
        /// </summary>
        /// <param name="attendees1">first attendee</param>
        /// <param name="attendees2">secons attendee</param>
        /// <param name="employees">Employee hierarchy collection</param>
        /// <returns></returns>
        private static int GetJunior(int attendees1, int attendees2, int[][] employees)
        {
            int jumpLevel1, jumpLevel2;

            jumpLevel1 = GetLevel(attendees1, employees);
            jumpLevel2 = GetLevel(attendees2, employees);


            if (jumpLevel1 == jumpLevel2)
            {
                return attendees1 > attendees2 ? attendees1 : attendees2;
            }
            else
            {
                return jumpLevel1 > jumpLevel2 ? attendees1 : attendees2;
            }

        }

        /// <summary>
        /// Gets the number of levels the employee is below the top manager
        /// </summary>
        /// <param name="employee">Employee</param>
        /// <param name="employees">Employee hierarchy collection</param>
        /// <param name="traversedLevel">OPTONAL:Levels traversed so far</param>
        /// <returns>Levels the employee need to jump</returns>
        private static int GetLevel(int employee, int[][] employees, int traversedLevel = 0)
        {
            try
            {
                var manager = employees.Where(i => i[0] == employee).Select(i => i[1]).Single();
                if (manager == employee) //Assuming Top most employee is his own manager. As per the shared data
                    return traversedLevel;
                else
                    return GetLevel(manager, employees, traversedLevel + 1);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Did not find manager for: " + employee.ToString());
                return -1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
