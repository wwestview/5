using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

[Serializable]
public struct Invoice : IComparable<Invoice>
{
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string CounterpartyName { get; set; }
    public string ContractNumber { get; set; }
    public int AmountInCents { get; set; }

    public int CompareTo(Invoice other)
    {
        return this.InvoiceNumber.CompareTo(other.InvoiceNumber);
    }

    public override string ToString()
    {
        return $"{InvoiceNumber}, {InvoiceDate:yyyy-MM-dd}, {CounterpartyName}, {ContractNumber}, {AmountInCents / 100} грн {AmountInCents % 100} коп";
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Console.WriteLine("Оберіть режим:");
        Console.WriteLine("1 - Ввести нові дані та зберегти у файл");
        Console.WriteLine("2 - Прочитати дані з файлу");

        int choice = int.Parse(Console.ReadLine());
        List<Invoice> invoices = new List<Invoice>();
            switch (choice)
            {
                case 1:
                    invoices = InputInvoices();
                    SaveInvoices(invoices);
                    break;
                case 2:
                    Console.WriteLine("Оберіть файл для читання:");
                    Console.WriteLine("1 - Текстовий файл");
                    Console.WriteLine("2 - XML файл");
                    Console.WriteLine("Щоб повернутися назад натисніть 0");
                    int fileChoice = int.Parse(Console.ReadLine());
                    if (fileChoice == 1)
                    {
                        invoices = LoadInvoicesFromText();
                    }
                    else if (fileChoice == 2)
                    {
                        invoices = LoadInvoicesFromXml();
                    }
                    SearchInvoices(invoices);
                    break;
                case 3:
                    Console.WriteLine("Натисніть (1) для того, щоб повернутися у початок програми або натисність (2), щоб повернутися на один крок назад");
                    if (choice == 1) goto case 1;
                    else if (choice == 2) goto case 2;
                    break;
                default:
                    Console.WriteLine("Неправильний вибір.");
                    break;
            }
       
    }

    static List<Invoice> InputInvoices()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        List<Invoice> invoices = new List<Invoice>();

        while (true)
        {
            Console.WriteLine("Введіть номер рахунку:");
            string number = Console.ReadLine();

            Console.WriteLine("Введіть дату оформлення (yyyy-mm-dd):");
            DateTime date = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Введіть назву контрагента:");
            string counterparty = Console.ReadLine();

            Console.WriteLine("Введіть номер контракту:");
            string contract = Console.ReadLine();

            Console.WriteLine("Введіть суму у копійках:");
            int amount = int.Parse(Console.ReadLine());

            invoices.Add(new Invoice
            {
                InvoiceNumber = number,
                InvoiceDate = date,
                CounterpartyName = counterparty,
                ContractNumber = contract,
                AmountInCents = amount
            });

            Console.WriteLine("Ввести ще один рахунок? (y/n)");
            if (Console.ReadLine().ToLower() != "y")
            {
                break;
            }
        }

        invoices.Sort();
        return invoices;
    }

    static void SaveInvoices(List<Invoice> invoices)
    {
        string textFilePath = "invoices.txt";
        string xmlFilePath = "invoices.xml";

        // Save as text
        using (StreamWriter writer = new StreamWriter(textFilePath))
        {
            foreach (var invoice in invoices)
            {
                writer.WriteLine($"{invoice.InvoiceNumber},{invoice.InvoiceDate:yyyy-MM-dd},{invoice.CounterpartyName},{invoice.ContractNumber},{invoice.AmountInCents}");
            }
        }

        // Save as XML
        XmlSerializer serializer = new XmlSerializer(typeof(List<Invoice>));
        using (FileStream fs = new FileStream(xmlFilePath, FileMode.Create))
        {
            serializer.Serialize(fs, invoices);
        }
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Console.WriteLine("Дані збережено у файли.");
    }

    static List<Invoice> LoadInvoicesFromText()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string textFilePath = "invoices.txt";
        List<Invoice> invoices = new List<Invoice>();

        if (File.Exists(textFilePath))
        {
            using (StreamReader reader = new StreamReader(textFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    invoices.Add(new Invoice
                    {
                        InvoiceNumber = parts[0],
                        InvoiceDate = DateTime.Parse(parts[1]),
                        CounterpartyName = parts[2],
                        ContractNumber = parts[3],
                        AmountInCents = int.Parse(parts[4])
                    });
                }
            }
        }
        else
        {

            Console.WriteLine("Файл не знайдено.");
        }

        return invoices;
    }

    static List<Invoice> LoadInvoicesFromXml()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string xmlFilePath = "invoices.xml";
        List<Invoice> invoices = new List<Invoice>();

        if (File.Exists(xmlFilePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Invoice>));
            using (FileStream fs = new FileStream(xmlFilePath, FileMode.Open))
            {
                invoices = (List<Invoice>)serializer.Deserialize(fs);
            }
        }
        else
        {
            Console.WriteLine("Файл не знайдено.");
        }

        return invoices;
    }

    static void SearchInvoices(List<Invoice> invoices)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Console.WriteLine("Введіть номер контракту для пошуку:");
        string contractNumber = Console.ReadLine();

        var filteredInvoices = invoices.FindAll(inv => inv.ContractNumber == contractNumber);

        if (filteredInvoices.Count > 0)
        {
            foreach (var invoice in filteredInvoices)
            {
                Console.WriteLine(invoice.ToString());
            }
        }
        else
        {
            Console.WriteLine("Рахунків з таким номером контракту не знайдено.");
        }
    }
}
