﻿using LinqTasks.Extensions;
using LinqTasks.Models;

namespace LinqTasks;

public static partial class Tasks
{
    public static IEnumerable<Emp> Emps { get; set; }
    public static IEnumerable<Dept> Depts { get; set; }

    static Tasks()
    {
        Depts = LoadDepts();
        Emps = LoadEmps();
    }

    /// <summary>
    ///     SELECT * FROM Emps WHERE Job = "Backend programmer";
    /// </summary>
    public static IEnumerable<Emp> Task1()
    {
        var result = from emp in Emps 
            where emp.Job == "Backend programmer" 
            select emp;

        return result;
    }

    /// <summary>
    ///     SELECT * FROM Emps Job = "Frontend programmer" AND Salary>1000 ORDER BY Ename DESC;
    /// </summary>
    public static IEnumerable<Emp> Task2()
    {
        var result = from emp in Emps 
            where emp.Job == "Frontend programmer" && emp.Salary > 1000 
            orderby emp.Ename descending 
            select emp;
        
        return result;
    }


    /// <summary>
    ///     SELECT MAX(Salary) FROM Emps;
    /// </summary>
    public static int Task3()
    {
        var result = Emps.Max(emp => emp.Salary);
        
        return result;
    }

    /// <summary>
    ///     SELECT * FROM Emps WHERE Salary=(SELECT MAX(Salary) FROM Emps);
    /// </summary>
    public static IEnumerable<Emp> Task4()
    {
        var result = Emps.Where(emp => emp.Salary == Task3());
            
        return result;
    }

    /// <summary>
    ///    SELECT ename AS Nazwisko, job AS Praca FROM Emps;
    /// </summary>
    public static IEnumerable<object> Task5()
    {
        var result = Emps.Select(emp => new
        {
            Nazwisko = emp.Ename,
            Praca = emp.Job
        });
            
        return result;
    }

    /// <summary>
    ///     SELECT Emps.Ename, Emps.Job, Depts.Dname FROM Emps
    ///     INNER JOIN Depts ON Emps.Deptno=Depts.Deptno
    ///     Rezultat: Złączenie kolekcji Emps i Depts.
    /// </summary>
    public static IEnumerable<object> Task6()
    {
        var result = Emps
            .Join(Depts, emp => emp.Deptno, dept => dept.Deptno, (emp, dept) => new {emp.Ename, emp.Job, dept.Dname});
        
        return result;
    }

    /// <summary>
    ///     SELECT Job AS Praca, COUNT(1) LiczbaPracownikow FROM Emps GROUP BY Job;
    /// </summary>
    public static IEnumerable<object> Task7()
    {
        var result = Emps.GroupBy(emp => emp.Job).Select((emp) => new
        {
            Praca = emp.Key,
            LiczbaPracownikow = emp.Count()
        });
            
        return result;
    }

    /// <summary>
    ///     Zwróć wartość "true" jeśli choć jeden
    ///     z elementów kolekcji pracuje jako "Backend programmer".
    /// </summary>
    public static bool Task8()
    {
        var result = Emps.Count(emp => emp.Job == "Backend programmer") > 0;
            
        return result;
    }

    /// <summary>
    ///     SELECT TOP 1 * FROM Emp WHERE Job="Frontend programmer"
    ///     ORDER BY HireDate DESC;
    /// </summary>
    public static Emp Task9()
    {
        var result =
            Emps.Where(emp => emp.Job == "Frontend programmer")
                .OrderByDescending(emp => emp.HireDate).First();
            
        return result;
    }

    /// <summary>
    ///     SELECT Ename, Job, Hiredate FROM Emps
    ///     UNION
    ///     SELECT "Brak wartości", null, null;
    /// </summary>
    public static IEnumerable<object> Task10()
    { 
        /*var result =
            Emps.Select(emp => new { emp.Ename, emp.Job, emp.HireDate })
                .Concat(new[]
                {
                    new { Ename = "Brak wartości", Job = (string)null, Hiredate = (DateTime?)null }
                });
            
        return result;*/
        return null;
    }

    /// <summary>
    ///     Wykorzystując LINQ pobierz pracowników podzielony na departamenty pamiętając, że:
    ///     1. Interesują nas tylko departamenty z liczbą pracowników powyżej 1
    ///     2. Chcemy zwrócić listę obiektów o następującej srukturze:
    ///     [
    ///     {name: "RESEARCH", numOfEmployees: 3},
    ///     {name: "SALES", numOfEmployees: 5},
    ///     ...
    ///     ]
    ///     3. Wykorzystaj typy anonimowe
    /// </summary>
    public static IEnumerable<object> Task11()
    {
        var result =
            Depts.GroupJoin(Emps, dept => dept.Deptno, emp => emp.Deptno,
                    (dept, emp) => new { dept, count = emp.Count() })
                .Where(entry => entry.count > 1)
                .Select(entry => new
                {
                    name = entry.dept.Dname,
                    numOfEmployees = entry.count
                });
        
        return result;
    }

    /// <summary>
    ///     Napisz własną metodę rozszerzeń, która pozwoli skompilować się poniższemu fragmentowi kodu.
    ///     Metodę dodaj do klasy CustomExtensionMethods, która zdefiniowana jest poniżej.
    ///     Metoda powinna zwrócić tylko tych pracowników, którzy mają min. 1 bezpośredniego podwładnego.
    ///     Pracownicy powinny w ramach kolekcji być posortowani po nazwisku (rosnąco) i pensji (malejąco).
    /// </summary>
    public static IEnumerable<Emp> Task12()
    {
        IEnumerable<Emp> result = Emps.GetEmpsWithSubordinates();
        
        return result;
    }

    /// <summary>
    ///     Poniższa metoda powinna zwracać pojedyczną liczbę int.
    ///     Na wejściu przyjmujemy listę liczb całkowitych.
    ///     Spróbuj z pomocą LINQ'a odnaleźć tę liczbę, które występuja w tablicy int'ów nieparzystą liczbę razy.
    ///     Zakładamy, że zawsze będzie jedna taka liczba.
    ///     Np: {1,1,1,1,1,1,10,1,1,1,1} => 10
    /// </summary>
    public static int Task13(int[] arr)
    {
        var result =
            arr.GroupBy(num => num)
                .Where(num => num.Count() % 2 != 0)
                .Select(entry => entry.Key).First();
        
        return result;
    }

    /// <summary>
    ///     Zwróć tylko te departamenty, które mają 5 pracowników lub nie mają pracowników w ogóle.
    ///     Posortuj rezultat po nazwie departament rosnąco.
    /// </summary>
    public static IEnumerable<Dept> Task14()
    {
        var result = 
            Depts.GroupJoin(Emps, dept => dept.Deptno, emp => emp.Deptno, 
                    (dept, emp) => new { dept, count = emp.Count() })
                .Where(entry => entry.count is 5 or 0)
                .Select(entry => entry.dept)
                .OrderBy(dept => dept.Dname);

        return result;
    }
}