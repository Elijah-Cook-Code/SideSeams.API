namespace SideSeams.API.DTOs
{
    public class MeasurementsDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public double? A_ChestMeasurement { get; set; }
        public double? B_SeatMeasurement { get; set; }
        public double? C_WaistMeasurement { get; set; }
        public double? D_TrouserMeasurement { get; set; }
        public double? E_F_HalfBackMeasurement { get; set; }
        public double? G_H_BackNeckToWaistMeasurement { get; set; }
        public double? G_I_SyceDepthMeasurement { get; set; }
        public double? I_L_SleeveLengthOnePieceMeasurement { get; set; }
        public double? E_I_SleeveLengthTwoPieceMeasurement { get; set; }
        public double? N_InsideLegMeasurement { get; set; }
        public double? P_Q_BodyRiseMeasurement { get; set; }
        public double? R_CloseWristMeasurement { get; set; }
    }
}
