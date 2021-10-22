namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum ChangeScoreOptions
    {
        [Description("Increase")]
        Increase,
        [Description("Decrease")]
        Decrease,
    }
}
