using Homework_Class03.Models.Domain;

namespace Homework_Class03
{
    public static class StaticDb
    {
        public static List<Book> Books = new List<Book>()
        {
            new Book
            {
                Id=1,
                Author = "Charlote Bronte",
                Title = "Jane Eyre"
            },
            new Book
            {
                Id=2,
                Author = "Dan Brown",
                Title = "Inferno"
            },
            new Book
            {
                Id=3,
                Author = "Nicholas Sparks",
                Title = "Veronica decides to die"
            },
            new Book
            {
                Id=4,
                Author = "Jane Austen",
                Title = "Pride and Prejudice"
            }
        };
    }
}
