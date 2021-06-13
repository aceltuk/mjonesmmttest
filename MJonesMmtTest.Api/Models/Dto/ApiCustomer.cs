using System;
using System.Text;

namespace MJonesMmtTest.Api.Models.Dto
{
    public class ApiCustomer
    {
        public string Email { get; set; }

        public string CustomerId { get; set; }

        public bool Website { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime LastLoggedIn { get; set; }

        public string HouseNumber { get; set; }

        public string Street { get; set; }

        public string Town { get; set; }

        public string Postcode { get; set; }

        public string PreferredLanguage { get; set; }

        public string ToAddress()
        {
            var first = true;
            var result = new StringBuilder();

            void Action(string line)
            {
                if (string.IsNullOrWhiteSpace(line)) return;
                if (!first)
                {
                    result.Append(", ");
                }
                else
                {
                    first = false;
                }

                result.Append(line);
            }

            Action(HouseNumber);
            Action(Street);
            Action(Town);
            Action(Postcode);
            return result.ToString();
        }
    }
}
