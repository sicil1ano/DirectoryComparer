using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareDirectories
{
    class Info : IEquatable<Info>
    {
        public bool SubFolder;
        private string name;
        private DateTime createdDate;
        private DateTime modifiedDate;

        public string Name {
            get {
                return name;
            }

            set {
                name = value;
            }
        }

        public string Path
        {
            get {
                return path;
            }
            set {
                path = value;
            }
        }
        private string path;

        public DateTime CreatedDate
        {
            get {
                return createdDate;
            }

            set {
                createdDate = value;
            }
        }

        public DateTime ModifiedDate
        {
            get {
                return modifiedDate;
            }

            set
            {
                modifiedDate = value;
            }
        }

        public bool Equals(Info other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return Name.Equals(other.Name) && Path.Equals(other.Path) && CreatedDate.Equals(other.CreatedDate) && ModifiedDate.Equals(other.ModifiedDate);
        }

        public override int GetHashCode()
        {
            int hashInfoElementName = Name == null ? 0 : Name.GetHashCode();

            int hashInfoElementPath = Path == null ? 0 : Path.GetHashCode();

            int hashInfoElementCreatedDate = CreatedDate == null ? 0 : CreatedDate.GetHashCode();

            int hashInfoElementeModifiedDate = ModifiedDate == null ? 0 : ModifiedDate.GetHashCode();

            return hashInfoElementName ^ hashInfoElementPath ^ hashInfoElementCreatedDate ^ hashInfoElementeModifiedDate;
        }
    }
}
