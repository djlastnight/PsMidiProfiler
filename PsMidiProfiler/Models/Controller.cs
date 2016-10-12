namespace PsMidiProfiler.Models
{
    using PsMidiProfiler.Enums;

    public class Controller
    {
        private readonly ControllerType type;

        private readonly ControllerCategory category;

        public Controller(ControllerType type, ControllerCategory category)
        {
            this.type = type;
            this.category = category;
        }

        public ControllerType Type
        {
            get
            {
                return this.type;
            }
        }

        public ControllerCategory Category
        {
            get
            {
                return this.category;
            }
        }
    }
}