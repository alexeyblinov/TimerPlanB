using System;

namespace PlanB.BL.Model
{
    [Serializable]
    public class Gender
    {
        /// <summary>
        /// Gender name.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Create a new gender.
        /// </summary>
        /// <param name="name"> gender name </param>
        public Gender(string name)
        {
            if (name == "M" || name == "F")
            {
                Name = name;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Enter: M or F.", nameof(name));
            }    
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
