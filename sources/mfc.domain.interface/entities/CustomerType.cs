namespace mfc.domain.entities {
    /// <summary>
    /// Категория посетителя/заявителя
    /// </summary>
    public class CustomerType : Entity {

    public virtual string Caption { get; set; }

        public static CustomerType Empty = new CustomerType { Id = -1, Caption = "<Не выбрана>" };
    }
}