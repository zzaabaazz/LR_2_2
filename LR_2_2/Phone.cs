namespace XML
{
    /// <summary>
    /// Телефонный контакт.
    /// </summary>
    public class Phone
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Модель.
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// фирма-изготовитель
        /// </summary>
        public string Specs { get; set; }
        /// <summary>
        /// характеристики
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// стоимость
        /// </summary>
        /// <returns> Имя. </returns>
        public override string ToString()
        {
            return Model;
        }
    }
}