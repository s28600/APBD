using REST_API.Models;

namespace REST_API.Databases;

public class Appointments
{
    public static readonly List<Appointment> _appointments =
    [
        new Appointment(DateTime.Today, 0, "Control appointment", 140), 
        new Appointment(DateTime.Today, 1, "Vaccination", 230)
    ];

    public static List<Appointment> GetById(int id)
    {
        List<Appointment> output = new();
        foreach (var appointment in _appointments)
        {
            if (appointment.Animal.Id == id)
            {
                output.Add(appointment);
            }
        }

        return output;
    }
}