using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Person
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string ContactInfo { get; }

    protected Person(int id, string name, int age, string contactInfo)
    {
        Id = id;
        Name = name;
        Age = age;
        ContactInfo = contactInfo;
    }

    public abstract void DisplayInfo();
}

