using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.ViewModels {
    public class HomeVM {

        //Note: DateOnly is buggy, in future projects DateTime should be used instead of DateOnly
        public IEnumerable<Villa>? VillaList { get; set; }
        public DateOnly CheckInDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? CheckOutDate { get; set; }
        public int Nights { get; set; }
    }
}
