using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AlarmClockV2
{
    static String Current_State = "Main";
    static List<DateTime> Alarms = new List<DateTime>();
    static bool Alarm_Status = false;
    static String Delete_Index = "";

    public static String Pull_Formatted_Time(DateTime Input_Time)
    {

        String[] months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        return (months[Input_Time.Month - 1] + " " + Input_Time.ToString("dd, yyyy") + "\n" + Input_Time.ToString("hh:mm tt"));

    }

    public static void Alarm_Status_Validator() {
        if (Alarms.Count == 0) { Alarm_Status = false; } else { Alarm_Status = true; }
    }

    public static void Refresh_Screen() {
    
        if (Current_State.Equals("Main"))
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Alarm Clock\n" + Pull_Formatted_Time(DateTime.Now) + "\n");
            if (Alarm_Status == false)
            {
                Console.WriteLine("\n" + "Press [B] to Set an Alarm.");
            }
            else
            {
                Console.WriteLine("Alarms Set For: \n");
                Alarm_Manager(1, ["hide"]);
                Console.WriteLine("\n Press [B] to Set an Alarm. \n Press [C] to Manage Alarms.");
            }

        }

        else if (Current_State.Equals("Alarm_Creator"))
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Alarm Creator");
            Console.WriteLine("Select a Time for the Alarm to Sound. \n\n");

            Console.SetCursorPosition(0, 2);

            Console.WriteLine("\n" + Alarms[Alarms.Count - 1].ToString("MM/dd/yy hh:mm tt") + "\n\nPress [up/down arrow] to change hour. Press [left/right arrow] to change minute.");
            Console.WriteLine("Press [Escape] To Exit. \n");
        }

        else if (Current_State.Equals("Alarm_Set_Off"))
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Alarm Set Off \nAlarm went off. Time to get up!\n" + $"Alarm For: {Pull_Formatted_Time(DateTime.Now)}\n\n");

            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Press [Escape] To Exit. \n\n");
        }

        else if (Current_State.Equals("Alarm_Manager_Menu"))
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Alarm Manager \n Active Alarms:\n");
            Alarm_Manager(1, [""]);
            Console.WriteLine("\n");
            Console.Write($"Enter which alarm you would like to delete: {Delete_Index}");
        }
    }

    public static void Input_Manager(ConsoleKeyInfo Pressed_Key)
    {
        Alarm_Status_Validator();

        if (Current_State.Equals("Main"))
        {
            if (Pressed_Key.Key == ConsoleKey.B)
            {
                Current_State = "Alarm_Creator";
                Alarms.Add(DateTime.Now.AddHours(1));
                Console.Clear();
            }

            else if (Pressed_Key.KeyChar == 'c' && Alarm_Status == true)
            {
                Current_State = "Alarm_Manager_Menu";
                Console.Clear();
                Alarm_Status_Validator();
            }

        }

        if (Current_State.Equals("Alarm_Creator"))
        {
            if (Pressed_Key.Key == ConsoleKey.LeftArrow) { Alarms[Alarms.Count - 1] = Alarms[Alarms.Count - 1].AddMinutes(-1); }
            if (Pressed_Key.Key == ConsoleKey.RightArrow) { Alarms[Alarms.Count - 1] = Alarms[Alarms.Count - 1].AddMinutes(1); }
            if (Pressed_Key.Key == ConsoleKey.UpArrow) { Alarms[Alarms.Count - 1] = Alarms[Alarms.Count - 1].AddHours(1); }
            if (Pressed_Key.Key == ConsoleKey.DownArrow) { Alarms[Alarms.Count - 1] = Alarms[Alarms.Count - 1].AddHours(-1); }
            if (Alarms[Alarms.Count - 1].CompareTo(DateTime.Now) < 1) { Alarms[Alarms.Count - 1] = DateTime.Now.AddMinutes(1); }

            if (Pressed_Key.Key == ConsoleKey.Escape) 
            {
                Console.Clear();
                Current_State = "Main";
                Alarms.RemoveAt(Alarms.Count - 1);
                Alarm_Status_Validator();
            }

            if (Pressed_Key.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                Current_State = "Main";
                Alarm_Status = true;
            }
        }

        if (Current_State.Equals("Alarm_Set_Off"))
        {
            if(Pressed_Key.Key == ConsoleKey.Escape)
            {
                Console.Clear();
                Current_State = "Main";
                //Alarms.RemoveAt(Index_Set_Off);
                Alarm_Status_Validator();
            }
        }

        if (Current_State.Equals("Alarm_Manager_Menu"))
        {
            
            if(Pressed_Key.Key == ConsoleKey.Escape)
            {
                Current_State = "Main";
                Console.Clear();
                Delete_Index = "";
            }

            else if(Pressed_Key.Key == ConsoleKey.Enter && !Delete_Index.Equals("") && int.Parse(Delete_Index) < Alarms.Count )
            {
                Alarms.RemoveAt(int.Parse(Delete_Index));
                Alarm_Status_Validator();
                Console.Clear();
                Delete_Index = "";
                Current_State = "Main";
            }

            else if(Pressed_Key.Key == ConsoleKey.Backspace && Delete_Index.Length > 0)
            {
                Console.Clear();
                Delete_Index = Delete_Index.Substring(0, Delete_Index.Length - 1);
            }

            else if (char.IsDigit(Pressed_Key.KeyChar))
            {
                Delete_Index += Pressed_Key.KeyChar;
            }

        }
    }

    public static string Alarm_Manager(int mode, string[]? settings = null) 
    {
        // Mode 0: Checks for matching alarm, Mode 1: Prints all alarms (returns ""), Mode 2: Returns the matching alarm's index,

        if (mode == 0) // Checks current time against all current alarms.
        {
            foreach (DateTime Alarm in Alarms)
            {
                if (DateTime.Now.ToString("MM/dd/yy hh:mm").Equals(Alarm.ToString("MM/dd/yy hh:mm"))) { return "true"; }
            }
            return "false";
        }

        else if (mode == 1) // Prints all alarms.
        {
            for(int i = 0; i < Alarms.Count; i++)
            {
                if (settings[0].Equals("hide")) { Console.WriteLine("\t" + Alarms[i].ToString("MM/dd/yy hh:mm")); }
                else { Console.WriteLine($"\t{i}: " + Alarms[i].ToString("MM/dd/yy hh:mm")); }
            }
            return "";
        }
        else if (mode == 2) // Returns the matching alarm's index. Must be converted to int from string.
        {
            for(int i = 0; i < Alarms.Count; i++)
            {
                if (DateTime.Now.ToString("MM/dd/yy hh:mm").Equals(Alarms[i].ToString("MM/dd/yy hh:mm"))) { return i.ToString(); }
            }
        }

        return "Alarm_Manager() Error: Invalid Mode."; // To return if nothing happened. Will execute if the int entered was not a valid mode.
    }

    static async Task Main(String[] args)
    {
        Console.CursorVisible = false;

        while (true)
        {
            if (Console.KeyAvailable) { Input_Manager(Console.ReadKey(intercept: true)); }
            Refresh_Screen();
            if (Alarm_Status && Alarm_Manager(0).Equals("true")) { if (!Current_State.Equals("Alarm_Set_Off")) { Current_State = "Alarm_Set_Off"; Console.Clear(); Alarms.RemoveAt(int.Parse(Alarm_Manager(2)));  } }
            await Task.Delay(100);
        }

    }

}