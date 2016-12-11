namespace mfc.domain.entities
{
    public class Operation
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Operation;
            if (other == null) return false;

            return Equals(other.Code, Code);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
