public class DataStorage
{
    public static string FilePath = "/Users/apple/RiderProjects/HospitalManagmentSystem/HospitalManagmentSystem/Datas/data.json";

    public static (List<User>, List<Department>) LoadData()
    {
        if (!File.Exists(FilePath))
            return (new List<User>(), new List<Department>());

        var json = File.ReadAllText(FilePath);

        if (string.IsNullOrWhiteSpace(json))
            return (new List<User>(), new List<Department>());

        var data = JsonSerializer.Deserialize<DataModel>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return (data?.Users ?? new List<User>(), data?.Departments ?? new List<Department>());
    }

    public static void SaveData(List<User> users, List<Department> departments)
    {
        var data = new DataModel
        {
            Users = users,
            Departments = departments,
        };

        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(FilePath, json);
        
        
    }
}

public class DataModel
{
    public List<User> Users { get; set; } = new();
    public List<Doctor> Doctors { get; set; } = new();
    public List<Department> Departments { get; set; } = new();
}