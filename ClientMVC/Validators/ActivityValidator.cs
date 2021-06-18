using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientMVC.Models;
using FluentValidation;
namespace ClientMVC.Validators
{
    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
               //.Must(BeAValidDate).WithMessage("Invalid date");
                //.GreaterThan(DateTime.Now.AddDays(1)).WithMessage("must be greater than today");

                // .Must(BeAValidDate).WithMessage("must be greater than today");

            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Venue).NotEmpty();
        }

        //private bool BeAValidDate(string value)
        //{
        //    DateTime date;
        //    return DateTime.TryParse(value, out date);
        //}
        //private bool BeAValidDate(DateTime date)
        //{
        //    return date> DateTime.Now.AddDays(1);
        //}
    }

}
