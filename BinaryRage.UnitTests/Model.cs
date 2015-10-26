using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryRage.UnitTests
{
    [Serializable]
    public class Model : IEquatable<Model>
    {
        public string Title { get; set; }
        public string ThumbUrl { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public bool Equals(Model other)
        {
            if (other == null) return false;

            return string.Equals(this.Title, other.Title)
                   && string.Equals(this.Description, other.Description)
                   && string.Equals(this.ThumbUrl, other.ThumbUrl)
                   && this.Price.Equals(other.Price);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as Model);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)15257167;

                hash = (hash * 10786583) ^ (!string.IsNullOrEmpty(this.Title) ? this.Title.GetHashCode() : 0);
                hash = (hash * 10786583) ^ (!string.IsNullOrEmpty(this.Description) ? this.Description.GetHashCode() : 0);
                hash = (hash * 10786583) ^ (!string.IsNullOrEmpty(this.ThumbUrl) ? this.ThumbUrl.GetHashCode() : 0);
                hash = (hash * 10786583) ^ this.Price.GetHashCode();

                return hash;
            }
        }
    }
}