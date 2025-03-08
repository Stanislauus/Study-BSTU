using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
namespace NetW
{
    class Program
    {
        static void Main()
        {
            var role = new List<Role> { new Role { Id = Guid.NewGuid(), Name = "User" } };
            var users = new List<User>
            {
                new User("Ivan", "Ivanov", 24)
                {
                    Roles = role,
                    Type = UserType.New,
                    Sex = 'M'
                },

                new User("Nikita", "Nikolaev", 24)
                {
                    Roles = role,
                    Type = UserType.New,
                    Sex = 'M'
                },
            };

            XmlSerializeWrapper.Serialize(users, "users.xml");
            var deserializeUsers = XmlSerializeWrapper.Deserialize<List<User>>("users.xml");

            XmlSerializeWrapper.Serialize(users.First(), "user.xml");
            var deserializeUser = XmlSerializeWrapper.Deserialize<User>("user.xml");
        }
    }

    [Serializable]
    [XmlRoot(Namespace = "NetW")]
    [XmlType("user")]
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        public User(string firstName, string lastName, int age) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        [XmlIgnore]
        public char Sex { get; set; }

        [XmlElement(ElementName = "id")]
        public Guid Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "surname")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "age")]
        public int Age { get; set; }

        [XmlElement(ElementName = "type")]
        public UserType Type { get; set; }

        [XmlArray("roles")]
        [XmlArrayItem("role")]
        public List<Role> Roles { get; set; }
    }

    [Serializable]
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    [Serializable]
    public enum UserType
    {
        [XmlEnum("L")]
        Locked,
        [XmlEnum("N")]
        New
    }

    public static class XmlSerializeWrapper
    {
        public static void Serialize<T>(T obj, string filename)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));

            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, obj);
            }
        }

        public static T Deserialize<T>(string filename)
        {
            T obj;
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                obj = (T)formatter.Deserialize(fs);
            }

            return obj;
        }
    }
}
