namespace CookWithMe.Web.Common.ValidationAttribures
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int minElements;

        public EnsureMinimumElementsAttribute(int minElements)
        {
            this.minElements = minElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;

            if (list != null)
            {
                return list.Count >= this.minElements;
            }

            return false;
        }
    }
}
