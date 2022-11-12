using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace TestApi.Application.Misc
{

    public class DateDTO
    {
        public DateDTO()
        {
        }

        public static DateDTO Create(
            DateTime date,
            DateTime dateTime)
        {
            return new DateDTO
            {
                Date = date,
                DateTime = dateTime,
            };
        }

        public DateTime Date { get; set; }

        public DateTime DateTime { get; set; }

    }
}