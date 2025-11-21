using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Painting;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Painting
{
    public static class PaintingAreaMapper
    {
        public static PaintingAreaMainDto ToMainDto(PaintingArea entity)
        {
            if (entity == null) return null;
            return new PaintingAreaMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                PaintingArea = new PaintingAreaDto
                {
                    Inside_Area_Casing_Area_Mm2 = entity.Inside_Area_Casing_Area_Mm2,
                    Inside_Area_Casing_Area_M2 = entity.Inside_Area_Casing_Area_M2,
                    Inside_Area_Hopper_Area_Mm2 = entity.Inside_Area_Hopper_Area_Mm2,
                    Inside_Area_Hopper_Area_M2 = entity.Inside_Area_Hopper_Area_M2,
                    Inside_Area_Air_Header_Mm2 = entity.Inside_Area_Air_Header_Mm2,
                    Inside_Area_Air_Header_M2 = entity.Inside_Area_Air_Header_M2,
                    Inside_Area_Purge_Pipe_Mm2 = entity.Inside_Area_Purge_Pipe_Mm2,
                    Inside_Area_Purge_Pipe_M2 = entity.Inside_Area_Purge_Pipe_M2,
                    Inside_Area_Roof_Door_Mm2 = entity.Inside_Area_Roof_Door_Mm2,
                    Inside_Area_Roof_Door_M2 = entity.Inside_Area_Roof_Door_M2,
                    Inside_Area_Tube_Sheet_Mm2 = entity.Inside_Area_Tube_Sheet_Mm2,
                    Inside_Area_Tube_Sheet_M2 = entity.Inside_Area_Tube_Sheet_M2,
                    Inside_Area_Total_M2 = entity.Inside_Area_Total_M2,
                    Outside_Area_Casing_Area_Mm2 = entity.Outside_Area_Casing_Area_Mm2,
                    Outside_Area_Casing_Area_M2 = entity.Outside_Area_Casing_Area_M2,
                    Outside_Area_Hopper_Area_Mm2 = entity.Outside_Area_Hopper_Area_Mm2,
                    Outside_Area_Hopper_Area_M2 = entity.Outside_Area_Hopper_Area_M2,
                    Outside_Area_Air_Header_Mm2 = entity.Outside_Area_Air_Header_Mm2,
                    Outside_Area_Air_Header_M2 = entity.Outside_Area_Air_Header_M2,
                    Outside_Area_Purge_Pipe_Mm2 = entity.Outside_Area_Purge_Pipe_Mm2,
                    Outside_Area_Purge_Pipe_M2 = entity.Outside_Area_Purge_Pipe_M2,
                    Outside_Area_Roof_Door_Mm2 = entity.Outside_Area_Roof_Door_Mm2,
                    Outside_Area_Roof_Door_M2 = entity.Outside_Area_Roof_Door_M2,
                    Outside_Area_Tube_Sheet_Mm2 = entity.Outside_Area_Tube_Sheet_Mm2,
                    Outside_Area_Tube_Sheet_M2 = entity.Outside_Area_Tube_Sheet_M2,
                    Outside_Area_Total_M2 = entity.Outside_Area_Total_M2,
                },

            };
        }

        public static PaintingArea ToEntity(PaintingAreaMainDto dto)
        {
            if (dto == null) return null;
            return new PaintingArea
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Inside_Area_Casing_Area_Mm2 = dto.PaintingArea.Inside_Area_Casing_Area_Mm2,
                Inside_Area_Casing_Area_M2 = dto.PaintingArea.Inside_Area_Casing_Area_M2,
                Inside_Area_Hopper_Area_Mm2 = dto.PaintingArea.Inside_Area_Hopper_Area_Mm2,
                Inside_Area_Hopper_Area_M2 = dto.PaintingArea.Inside_Area_Hopper_Area_M2,
                Inside_Area_Air_Header_Mm2 = dto.PaintingArea.Inside_Area_Air_Header_Mm2,
                Inside_Area_Air_Header_M2 = dto.PaintingArea.Inside_Area_Air_Header_M2,
                Inside_Area_Purge_Pipe_Mm2 = dto.PaintingArea.Inside_Area_Purge_Pipe_Mm2,
                Inside_Area_Purge_Pipe_M2 = dto.PaintingArea.Inside_Area_Purge_Pipe_M2,
                Inside_Area_Roof_Door_Mm2 = dto.PaintingArea.Inside_Area_Roof_Door_Mm2,
                Inside_Area_Roof_Door_M2 = dto.PaintingArea.Inside_Area_Roof_Door_M2,
                Inside_Area_Tube_Sheet_Mm2 = dto.PaintingArea.Inside_Area_Tube_Sheet_Mm2,
                Inside_Area_Tube_Sheet_M2 = dto.PaintingArea.Inside_Area_Tube_Sheet_M2,
                Inside_Area_Total_M2 = dto.PaintingArea.Inside_Area_Total_M2,
                Outside_Area_Casing_Area_Mm2 = dto.PaintingArea.Outside_Area_Casing_Area_Mm2,
                Outside_Area_Casing_Area_M2 = dto.PaintingArea.Outside_Area_Casing_Area_M2,
                Outside_Area_Hopper_Area_Mm2 = dto.PaintingArea.Outside_Area_Hopper_Area_Mm2,
                Outside_Area_Hopper_Area_M2 = dto.PaintingArea.Outside_Area_Hopper_Area_M2,
                Outside_Area_Air_Header_Mm2 = dto.PaintingArea.Outside_Area_Air_Header_Mm2,
                Outside_Area_Air_Header_M2 = dto.PaintingArea.Outside_Area_Air_Header_M2,
                Outside_Area_Purge_Pipe_Mm2 = dto.PaintingArea.Outside_Area_Purge_Pipe_Mm2,
                Outside_Area_Purge_Pipe_M2 = dto.PaintingArea.Outside_Area_Purge_Pipe_M2,
                Outside_Area_Roof_Door_Mm2 = dto.PaintingArea.Outside_Area_Roof_Door_Mm2,
                Outside_Area_Roof_Door_M2 = dto.PaintingArea.Outside_Area_Roof_Door_M2,
                Outside_Area_Tube_Sheet_Mm2 = dto.PaintingArea.Outside_Area_Tube_Sheet_Mm2,
                Outside_Area_Tube_Sheet_M2 = dto.PaintingArea.Outside_Area_Tube_Sheet_M2,
                Outside_Area_Total_M2 = dto.PaintingArea.Outside_Area_Total_M2,

            };
        }
    }
}
