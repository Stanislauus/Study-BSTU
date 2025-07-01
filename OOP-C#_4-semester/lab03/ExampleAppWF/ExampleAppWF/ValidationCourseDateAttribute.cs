using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleAppWF.ValidationAttributes
{
        // Кастомный атрибут для проверки даты курса
        public class ValidationCourseDateAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is DateTime date)
                {
                    var now = DateTime.Now;

                    if (date < now.Date)
                        return new ValidationResult("Дата начала курса не может быть в прошлом.");

                    if (date > now.AddYears(1).Date)
                        return new ValidationResult("Дата начала курса не может быть позже чем через год.");

                    return ValidationResult.Success;
                }

                return new ValidationResult("Некорректная дата курса.");
            }
        }
}
