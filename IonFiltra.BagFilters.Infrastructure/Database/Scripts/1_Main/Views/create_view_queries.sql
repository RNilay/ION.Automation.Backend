CREATE OR REPLACE VIEW ionfiltrabagfilters.vw_WeightSummary AS
SELECT distinct
    ws.*, 
    md.BagFilterName
FROM 
    ionfiltrabagfilters.weightsummary AS ws
JOIN 
    ionfiltrabagfilters.bagfiltermaster AS md 
    ON ws.EnquiryId = md.EnquiryId and ws.BagfilterMasterId = md.BagfilterMasterId;

GO

CREATE OR REPLACE VIEW ionfiltrabagfilters.vw_BagfilterDetails AS
WITH DistinctPI AS (
    -- only distinct by Enquiry + Process_Volume_M3h (no BagfilterMasterId)
    SELECT DISTINCT
        EnquiryId,
        Process_Volume_M3h
    FROM ionfiltrabagfilters.ProcessInfo
    WHERE Process_Volume_M3h IS NOT NULL
),
-- new: counts of BagfilterInput per Enquiry + Process_Volume_M3h
VolumeCounts AS (
    SELECT
        EnquiryId,
        Process_Volume_M3h,
        COUNT(*) AS Qty
    FROM ionfiltrabagfilters.BagfilterInput
    WHERE Process_Volume_M3h IS NOT NULL
    GROUP BY EnquiryId, Process_Volume_M3h
)
SELECT
    -- Enquiry
    E.Id                                 AS EnquiryId,
    E.UserId                             AS Enquiry_UserId,
    E.EnquiryId                          AS Enquiry_ExternalId,
    E.Customer                           AS Enquiry_Customer,
    E.RequiredBagFilters                 AS Enquiry_RequiredBagFilters,
    E.ProcessVolumes                     AS Enquiry_ProcessVolumes,

    -- BagfilterMaster (we pick one BagfilterMasterId per Enquiry+Volume via PickBM)
    BM.BagfilterMasterId                 AS BagfilterMaster_BagfilterMasterId,
    BM.AssignmentId                      AS BagfilterMaster_AssignmentId,
    BM.EnquiryId                         AS BagfilterMaster_EnquiryId,
    BM.BagFilterName                     AS BagfilterMaster_BagFilterName,
    BM.Status                            AS BagfilterMaster_Status,
    BM.Revision                          AS BagfilterMaster_Revision,

    -- WeightSummary
    WS.Id                                AS WeightSummary_Id,
    WS.EnquiryId                         AS WeightSummary_EnquiryId,
    WS.BagfilterMasterId                 AS WeightSummary_BagfilterMasterId,
    WS.Casing_Weight,
    WS.Capsule_Weight,
    WS.Tot_Weight_Per_Compartment,
    WS.Hopper_Weight,
    WS.Weight_Of_Cage_Ladder,
    WS.Railing_Weight,
    WS.Tubesheet_Weight                  AS WeightSummary_Tubesheet_Weight,
    WS.Air_Header_Blow_Pipe,
    WS.Hopper_Access_Stool_Weight,
    WS.Weight_Of_Mid_Landing_Plt,
    WS.Weight_Of_Maintainence_Pltform,
    WS.Cage_Weight                       AS WeightSummary_Cage_Weight,
    WS.Structure_Weight,
    WS.Scrap_Holes_Weight,
    WS.Weight_Total                      AS WeightSummary_Weight_Total,

    -- ProcessInfo (for that distinct volume)
    PI.Id                                AS ProcessInfo_Id,
    PI.EnquiryId                         AS ProcessInfo_EnquiryId,
    PI.BagfilterMasterId                 AS ProcessInfo_BagfilterMasterId,
    PI.Process_Volume_M3h,
    PI.Design_Pressure_Mmwc,
    -- NEW: Qty for this Enquiry + Process_Volume_M3h (counts from BagfilterInput)
    COALESCE(VC.Qty, 0)                  AS Qty,
    PI.Mfg_Plant,
    PI.Destination_State,
    PI.Location,
    PI.ProcessVolumeMin,
    PI.Process_Acrmax,
    PI.ClothArea,
    PI.Process_Dust,
    PI.Process_Dustload_gmspm3,
    PI.Process_Temp_C,
    PI.Dew_Point_C,
    PI.Outlet_Emission_mgpm3,
    PI.Process_Cloth_Ratio,
    PI.Can_Correction,
    PI.Customer_Equipment_Tag_No,
    PI.Bagfilter_Cleaning_Type,
    PI.Offline_Maintainence,
    PI.Bag_Filter_Capacity_V,
    PI.Process_Vol_M3_Sec,
    PI.Process_Vol_M3_Min,
    PI.Bag_Area,
    PI.Bag_Bottom_Area,
    PI.Min_Cloth_Area_Req,
    PI.Min_Bag_Req,

    -- CageInputs
    CI.Id                                AS CageInputs_Id,
    CI.EnquiryId                         AS CageInputs_EnquiryId,
    CI.BagfilterMasterId                 AS CageInputs_BagfilterMasterId,
    CI.Cage_Type,
    CI.Cage_Sub_Type,
    CI.Cage_Material,
    CI.Cage_Wire_Dia,
    CI.No_Of_Cage_Wires,
    CI.Ring_Spacing,
    CI.Cage_Diameter,
    CI.Cage_Length,
    CI.Spare_Cages,
    CI.Cage_Configuration,

    -- BagSelection
    BS.Id                                AS BagSelection_Id,
    BS.EnquiryId                         AS BagSelection_EnquiryId,
    BS.BagfilterMasterId                 AS BagSelection_BagfilterMasterId,
    BS.Filter_Bag_Dia,
    BS.Fil_Bag_Length,
    BS.ClothAreaPerBag,
    BS.noOfBags,
    BS.Fil_Bag_Recommendation,
    BS.Bag_Per_Row,
    BS.Number_Of_Rows,
    BS.Actual_Bag_Req,
    BS.Wire_Cross_Sec_Area,
    BS.No_Of_Rings,
    BS.Tot_Wire_Length,
    BS.Cage_Weight                       AS BagSelection_Cage_Weight,

    -- StructureInputs
    SI.Id                                AS StructureInputs_Id,
    SI.EnquiryId                         AS StructureInputs_EnquiryId,
    SI.BagfilterMasterId                 AS StructureInputs_BagfilterMasterId,
    SI.Gas_Entry,
    SI.Support_Structure_Type,
    SI.Nominal_Width,
    SI.Max_Bags_And_Pitch,
    SI.Nominal_Width_Meters,
    SI.Nominal_Length,
    SI.Nominal_Length_Meters,
    SI.Area_Adjust_Can_Vel,
    SI.Can_Area_Req,
    SI.Total_Avl_Area,
    SI.Length_Correction,
    SI.Length_Correction_Derived,
    SI.Actual_Length,
    SI.Actual_Length_Meters,
    SI.Ol_Flange_Length,
    SI.Ol_Flange_Length_Mm,

    -- CapsuleInputs
    CAPS.Id                              AS CapsuleInputs_Id,
    CAPS.EnquiryId                       AS CapsuleInputs_EnquiryId,
    CAPS.BagfilterMasterId               AS CapsuleInputs_BagfilterMasterId,
    CAPS.Valve_Size,
    CAPS.Voltage_Rating,
    CAPS.Capsule_Height,
    CAPS.Tube_Sheet_Thickness,
    CAPS.Capsule_Wall_Thickness,
    CAPS.Canopy,
    CAPS.Solenoid_Valve_Maintainence,
    CAPS.Capsule_Area,
    CAPS.Capsule_Weight                 AS CapsuleInputs_Capsule_Weight,
    CAPS.Tubesheet_Area,
    CAPS.Tubesheet_Weight,

    -- CasingInputs
    CAS.Id                               AS CasingInputs_Id,
    CAS.EnquiryId                        AS CasingInputs_EnquiryId,
    CAS.BagfilterMasterId                AS CasingInputs_BagfilterMasterId,
    CAS.Casing_Wall_Thickness,
    CAS.Casing_Height,
    CAS.Casing_Area,
    CAS.Casing_Weight                    AS CasingInputs_Casing_Weight,

    -- HopperInputs
    HOP.Id                               AS HopperInputs_Id,
    HOP.EnquiryId                        AS HopperInputs_EnquiryId,
    HOP.BagfilterMasterId                AS HopperInputs_BagfilterMasterId,
    HOP.Hopper_Type,
    HOP.Process_Compartments,
    HOP.Tot_No_Of_Hoppers,
    HOP.Tot_No_Of_Trough,
    HOP.Plenum_Width,
    HOP.Inlet_Height,
    HOP.Hopper_Thickness,
    HOP.Hopper_Valley_Angle,
    HOP.Access_Door_Type,
    HOP.Access_Door_Qty,
    HOP.Rav_Maintainence_Pltform,
    HOP.Hopper_Access_Stool,
    HOP.Is_Distance_Piece,
    HOP.Distance_Piece_Height,
    HOP.Stiffening_Factor,
    HOP.Hopper,
    HOP.Discharge_Opening_Sqr,
    HOP.Rav_Height,
    HOP.Material_Handling,
    HOP.Material_Handling_Qty,
    HOP.Trough_Outlet_Length,
    HOP.Trough_Outlet_Width,
    HOP.Material_Handling_Xxx,
    HOP.Hor_Diff_Length,
    HOP.Hor_Diff_Width,
    HOP.Slant_Offset_Dist,
    HOP.Hopper_Height,
    HOP.Hopper_Height_Mm,
    HOP.Slanting_Hopper_Height,
    HOP.Hopper_Area_Length,
    HOP.Hopper_Area_Width,
    HOP.Hopper_Tot_Area,
    HOP.Hopper_Weight                     AS HopperInputs_Hopper_Weight,

    -- SupportStructure
    SS.Id                                AS SupportStructure_Id,
    SS.EnquiryId                         AS SupportStructure_EnquiryId,
    SS.BagfilterMasterId                 AS SupportStructure_BagfilterMasterId,
    SS.Support_Struct_Type,
    SS.NoOfColumn,
    SS.Column_Height,
    SS.Ground_Clearance,
    SS.Slide_Gate_Height,
    SS.Dist_Btw_Column_In_X,
    SS.Dist_Btw_Column_In_Z,
    SS.No_Of_Bays_In_X,
    SS.No_Of_Bays_In_Z,

    -- AccessGroup
    AG.Id                                AS AccessGroup_Id,
    AG.EnquiryId                         AS AccessGroup_EnquiryId,
    AG.BagfilterMasterId                 AS AccessGroup_BagfilterMasterId,
    AG.Access_Type,
    AG.Cage_Weight_Ladder,
    AG.Mid_Landing_Pltform,
    AG.Platform_Weight,
    AG.Staircase_Height,
    AG.Staircase_Weight,
    AG.Railing_Weight                     AS AccessGroup_Railing_Weight,
    AG.Maintainence_Pltform,
    AG.Maintainence_Pltform_Weight,
    AG.BlowPipe,
    AG.PressureHeader,
    AG.DistancePiece,
    AG.Access_Stool_Size_Mm,
    AG.Access_Stool_Size_Kg,

    -- RoofDoor
    RD.Id                                AS RoofDoor_Id,
    RD.EnquiryId                         AS RoofDoor_EnquiryId,
    RD.BagfilterMasterId                 AS RoofDoor_BagfilterMasterId,
    RD.Roof_Door_Thickness,
    RD.T2d,
    RD.T3d,
    RD.N_Doors,
    RD.Compartment_No,
    RD.Stiffness_Factor_For_Roof_Door,
    RD.Weight_Per_Door,
    RD.Tot_Weight_Per_Compartment         AS RoofDoor_Tot_Weight_Per_Compartment,
    
    -- PaintingArea
    PA.Id                                 AS PaintingArea_Id,
    PA.EnquiryId                          AS PaintingArea_EnquiryId,
    PA.BagfilterMasterId                  AS PaintingArea_BagfilterMasterId,
    PA.Inside_Area_Casing_Area_Mm2,
	PA.Inside_Area_Casing_Area_M2,
	PA.Inside_Area_Hopper_Area_Mm2,
	PA.Inside_Area_Hopper_Area_M2,
	PA.Inside_Area_Air_Header_Mm2,
	PA.Inside_Area_Air_Header_M2,
	PA.Inside_Area_Purge_Pipe_Mm2,
	PA.Inside_Area_Purge_Pipe_M2,
	PA.Inside_Area_Roof_Door_Mm2,
	PA.Inside_Area_Roof_Door_M2,
	PA.Inside_Area_Tube_Sheet_Mm2,
	PA.Inside_Area_Tube_Sheet_M2,
	PA.Inside_Area_Total_M2,
	PA.Outside_Area_Casing_Area_Mm2,
	PA.Outside_Area_Casing_Area_M2,
	PA.Outside_Area_Hopper_Area_Mm2,
	PA.Outside_Area_Hopper_Area_M2,
	PA.Outside_Area_Air_Header_Mm2,
	PA.Outside_Area_Air_Header_M2,
	PA.Outside_Area_Purge_Pipe_Mm2,
	PA.Outside_Area_Purge_Pipe_M2,
	PA.Outside_Area_Roof_Door_Mm2,
	PA.Outside_Area_Roof_Door_M2,
	PA.Outside_Area_Tube_Sheet_Mm2,
	PA.Outside_Area_Tube_Sheet_M2,
	PA.Outside_Area_Total_M2

FROM DistinctPI D
-- join Enquiry so we keep Enquiry fields
LEFT JOIN ionfiltrabagfilters.Enquiry E
    ON D.EnquiryId = E.Id

-- pick one BagfilterMasterId per Enquiry+Volume (you can change MIN to MAX or other rule)
LEFT JOIN (
    SELECT
        EnquiryId,
        Process_Volume_M3h,
        MIN(BagfilterMasterId) AS PickBM
    FROM ionfiltrabagfilters.ProcessInfo
    WHERE Process_Volume_M3h IS NOT NULL
    GROUP BY EnquiryId, Process_Volume_M3h
) PickBM
  ON PickBM.EnquiryId = D.EnquiryId
 AND PickBM.Process_Volume_M3h = D.Process_Volume_M3h

LEFT JOIN ionfiltrabagfilters.BagfilterMaster BM
    ON BM.BagfilterMasterId = PickBM.PickBM

-- join ProcessInfo rows that match that distinct volume (if multiple PI rows share same volume and same bagfilter we will get PI rows;
-- if you want only a single PI row per volume you can replace this with another pick-subquery)
LEFT JOIN ionfiltrabagfilters.ProcessInfo PI
    ON PI.BagfilterMasterId = BM.BagfilterMasterId
    AND PI.EnquiryId = D.EnquiryId
    AND PI.Process_Volume_M3h = D.Process_Volume_M3h

-- join the Qty counts
LEFT JOIN VolumeCounts VC
    ON VC.EnquiryId = D.EnquiryId
   AND VC.Process_Volume_M3h = D.Process_Volume_M3h

LEFT JOIN ionfiltrabagfilters.WeightSummary WS
    ON WS.BagfilterMasterId = BM.BagfilterMasterId
    AND WS.EnquiryId = D.EnquiryId

LEFT JOIN ionfiltrabagfilters.CageInputs CI
    ON CI.BagfilterMasterId = BM.BagfilterMasterId
    AND CI.EnquiryId = D.EnquiryId

LEFT JOIN ionfiltrabagfilters.BagSelection BS
    ON BS.BagfilterMasterId = BM.BagfilterMasterId
    AND BS.EnquiryId = D.EnquiryId

LEFT JOIN ionfiltrabagfilters.StructureInputs SI
    ON SI.BagfilterMasterId = BM.BagfilterMasterId
    AND SI.EnquiryId = D.EnquiryId

LEFT JOIN ionfiltrabagfilters.CapsuleInputs CAPS
    ON CAPS.BagfilterMasterId = BM.BagfilterMasterId
    AND CAPS.EnquiryId = D.EnquiryId

LEFT JOIN ionfiltrabagfilters.CasingInputs CAS
    ON CAS.BagfilterMasterId = BM.BagfilterMasterId
    AND CAS.EnquiryId = D.EnquiryId

LEFT JOIN ionfiltrabagfilters.HopperInputs HOP
    ON HOP.BagfilterMasterId = BM.BagfilterMasterId
    AND HOP.EnquiryId = D.EnquiryId

LEFT JOIN ionfiltrabagfilters.SupportStructure SS
    ON SS.BagfilterMasterId = BM.BagfilterMasterId
    AND SS.EnquiryId = D.EnquiryId

LEFT JOIN ionfiltrabagfilters.AccessGroup AG
    ON AG.BagfilterMasterId = BM.BagfilterMasterId
    AND AG.EnquiryId = D.EnquiryId

LEFT JOIN ionfiltrabagfilters.RoofDoor RD
    ON RD.BagfilterMasterId = BM.BagfilterMasterId
    AND RD.EnquiryId = D.EnquiryId
    
LEFT JOIN ionfiltrabagfilters.paintingArea PA
    ON PA.BagfilterMasterId = BM.BagfilterMasterId
    AND PA.EnquiryId = D.EnquiryId;
GO



---- Gropu by Volumes view

    CREATE OR REPLACE VIEW ionfiltrabagfilters.vw_EnquiryVolumeSummary AS
WITH DistinctInputs AS (
    -- all distinct rows from BagfilterInput (use the real table name)
    SELECT
        EnquiryId,
        BagfilterMasterId,
        Process_Volume_M3h
    FROM ionfiltrabagfilters.BagfilterInput
    WHERE Process_Volume_M3h IS NOT NULL
),
Volumes AS (
    -- group by enquiry + volume across all bagfilters, count qty
    SELECT
        EnquiryId,
        Process_Volume_M3h,
        COUNT(*) AS Qty,
        GROUP_CONCAT(DISTINCT BagfilterMasterId) AS BagfilterMasterIds -- for debugging/inspection if needed
    FROM ionfiltrabagfilters.BagfilterInput
    WHERE Process_Volume_M3h IS NOT NULL
    GROUP BY EnquiryId, Process_Volume_M3h
),
VolumeWeights AS (
    -- sum Weight_Total from WeightSummary but only for bagfilters that have that volume
    SELECT
        V.EnquiryId,
        V.Process_Volume_M3h,
        SUM(WS.Weight_Total) AS WeightSum
    FROM Volumes V
    JOIN ionfiltrabagfilters.WeightSummary WS
      ON WS.EnquiryId = V.EnquiryId
      AND WS.BagfilterMasterId IN (
          -- pick bagfiltermaster ids that correspond to this enquiry+volume
          SELECT DISTINCT BagfilterMasterId
          FROM ionfiltrabagfilters.BagfilterInput BI
          WHERE BI.EnquiryId = V.EnquiryId
            AND BI.Process_Volume_M3h = V.Process_Volume_M3h
      )
    GROUP BY V.EnquiryId, V.Process_Volume_M3h
)
SELECT
    E.Id                       AS EnquiryId,
    E.EnquiryId                AS Enquiry_ExternalId,
    V.Process_Volume_M3h       AS Volume_M3h,
    V.Qty                      AS Qty,
    COALESCE(W.WeightSum, 0)   AS Weight,
    NULL                       AS Fab_Cost,
    NULL                       AS BoughtOut,
    NULL                       AS Total
FROM Volumes V
JOIN ionfiltrabagfilters.Enquiry E
  ON E.Id = V.EnquiryId
LEFT JOIN VolumeWeights W
  ON W.EnquiryId = V.EnquiryId
 AND W.Process_Volume_M3h = V.Process_Volume_M3h;

 --- Bill Of Material Details View

 CREATE OR REPLACE VIEW ionfiltrabagfilters.vw_BillOfMaterialDetails AS
WITH PiBm AS (
    SELECT
        e.Id                 AS EnquiryId,
        bm.BagfilterMasterId AS BagfilterMasterId,
        pi.Process_Volume_M3h,
        e.RequiredBagFilters AS Enquiry_RequiredBagFilters,

        -- choose a single BagfilterMaster per (Enquiry, Process_Volume_M3h)
        ROW_NUMBER() OVER (
            PARTITION BY e.Id, pi.Process_Volume_M3h
            ORDER BY bm.BagfilterMasterId
        ) AS RnPerVolume
    FROM ionfiltrabagfilters.ProcessInfo      pi
    JOIN ionfiltrabagfilters.BagfilterMaster bm
          ON bm.BagfilterMasterId = pi.BagfilterMasterId
         AND bm.EnquiryId          = pi.EnquiryId
    JOIN ionfiltrabagfilters.Enquiry         e
          ON e.Id                  = pi.EnquiryId
    WHERE pi.Process_Volume_M3h IS NOT NULL
),
DistinctVolumes AS (
    SELECT
        EnquiryId,
        BagfilterMasterId,
        Process_Volume_M3h,
        Enquiry_RequiredBagFilters,

        -- Qty = running number of *distinct* process volumes for this enquiry
        ROW_NUMBER() OVER (
            PARTITION BY EnquiryId
            ORDER BY Process_Volume_M3h, BagfilterMasterId
        ) AS Qty
    FROM PiBm
    WHERE RnPerVolume = 1  -- keep only one BagfilterMaster per volume
)
SELECT
    dv.EnquiryId,
    dv.BagfilterMasterId,
    dv.Process_Volume_M3h,
    dv.Enquiry_RequiredBagFilters,
    dv.Qty,

    -- Bill of Material line (only for the chosen BagfilterMasterId)
    bom.Item,
    bom.Material,
    bom.Weight,
    bom.Units,
    bom.Rate,
    bom.Cost,
    bom.SortOrder
FROM DistinctVolumes dv
JOIN ionfiltrabagfilters.BillOfMaterial bom
      ON bom.EnquiryId         = dv.EnquiryId
     AND bom.BagfilterMasterId = dv.BagfilterMasterId
ORDER BY
    dv.EnquiryId,
    dv.Process_Volume_M3h,
    dv.BagfilterMasterId,
    bom.SortOrder;



    ------Painting COst view

    CREATE OR REPLACE VIEW ionfiltrabagfilters.vw_PaintingCostDetails AS
WITH PiBm AS (
    SELECT
        e.Id                 AS EnquiryId,
        bm.BagfilterMasterId AS BagfilterMasterId,
        pi.Process_Volume_M3h,
        e.RequiredBagFilters AS Enquiry_RequiredBagFilters,

        -- choose a single BagfilterMaster per (Enquiry, Process_Volume_M3h)
        ROW_NUMBER() OVER (
            PARTITION BY e.Id, pi.Process_Volume_M3h
            ORDER BY bm.BagfilterMasterId
        ) AS RnPerVolume
    FROM ionfiltrabagfilters.ProcessInfo      pi
    JOIN ionfiltrabagfilters.BagfilterMaster bm
          ON bm.BagfilterMasterId = pi.BagfilterMasterId
         AND bm.EnquiryId          = pi.EnquiryId
    JOIN ionfiltrabagfilters.Enquiry         e
          ON e.Id                  = pi.EnquiryId
    WHERE pi.Process_Volume_M3h IS NOT NULL
),
DistinctVolumes AS (
    SELECT
        EnquiryId,
        BagfilterMasterId,
        Process_Volume_M3h,
        Enquiry_RequiredBagFilters,

        -- running number of DISTINCT process volumes for this enquiry
        ROW_NUMBER() OVER (
            PARTITION BY EnquiryId
            ORDER BY Process_Volume_M3h, BagfilterMasterId
        ) AS Qty
    FROM PiBm
    WHERE RnPerVolume = 1      -- keep only one BagfilterMaster per volume
)
SELECT
    dv.EnquiryId,
    dv.BagfilterMasterId,
    dv.Process_Volume_M3h,
    dv.Enquiry_RequiredBagFilters,
    dv.Qty,
    pc.Id               AS PaintingCostId,
    pc.PaintingTableJson
FROM DistinctVolumes dv
JOIN ionfiltrabagfilters.PaintingCost pc
      ON pc.EnquiryId         = dv.EnquiryId
     AND pc.BagfilterMasterId = dv.BagfilterMasterId
ORDER BY
    dv.EnquiryId,
    dv.Process_Volume_M3h,
    dv.BagfilterMasterId;

    -----transportation cost view--:

    
CREATE OR REPLACE VIEW ionfiltrabagfilters.vw_TransportationCostDetails AS
WITH PiBm AS (
    SELECT
        e.Id                 AS EnquiryId,
        bm.BagfilterMasterId AS BagfilterMasterId,
        pi.Process_Volume_M3h,
        e.RequiredBagFilters AS Enquiry_RequiredBagFilters,

        -- choose a single BagfilterMaster per (Enquiry, Process_Volume_M3h)
        ROW_NUMBER() OVER (
            PARTITION BY e.Id, pi.Process_Volume_M3h
            ORDER BY bm.BagfilterMasterId
        ) AS RnPerVolume
    FROM ionfiltrabagfilters.ProcessInfo pi
    JOIN ionfiltrabagfilters.BagfilterMaster bm
          ON bm.BagfilterMasterId = pi.BagfilterMasterId
         AND bm.EnquiryId          = pi.EnquiryId
    JOIN ionfiltrabagfilters.Enquiry e
          ON e.Id = pi.EnquiryId
    WHERE pi.Process_Volume_M3h IS NOT NULL
),
DistinctVolumes AS (
    SELECT
        EnquiryId,
        BagfilterMasterId,
        Process_Volume_M3h,
        Enquiry_RequiredBagFilters,

        -- Qty = running number of distinct process volumes per enquiry
        ROW_NUMBER() OVER (
            PARTITION BY EnquiryId
            ORDER BY Process_Volume_M3h, BagfilterMasterId
        ) AS Qty
    FROM PiBm
    WHERE RnPerVolume = 1
)
SELECT
    dv.EnquiryId,
    dv.BagfilterMasterId,
    dv.Process_Volume_M3h,
    dv.Enquiry_RequiredBagFilters,
    dv.Qty,

    -- Transportation Cost rows
    tc.Parameter,
    tc.Value,
    tc.Unit
FROM DistinctVolumes dv
JOIN ionfiltrabagfilters.TransportationCostEntity tc
      ON tc.EnquiryId         = dv.EnquiryId
     AND tc.BagfilterMasterId = dv.BagfilterMasterId
ORDER BY
    dv.EnquiryId,
    dv.Process_Volume_M3h,
    dv.BagfilterMasterId,
    tc.Id;


    ---damper cost view

    CREATE OR REPLACE VIEW ionfiltrabagfilters.vw_DamperCostDetails AS
WITH PiBm AS (
    SELECT
        e.Id AS EnquiryId,
        bm.BagfilterMasterId,
        pi.Process_Volume_M3h,
        e.RequiredBagFilters AS Enquiry_RequiredBagFilters,
        ROW_NUMBER() OVER (
            PARTITION BY e.Id, pi.Process_Volume_M3h
            ORDER BY bm.BagfilterMasterId
        ) AS RnPerVolume
    FROM ionfiltrabagfilters.ProcessInfo pi
    JOIN ionfiltrabagfilters.BagfilterMaster bm
      ON bm.BagfilterMasterId = pi.BagfilterMasterId
     AND bm.EnquiryId = pi.EnquiryId
    JOIN ionfiltrabagfilters.Enquiry e
      ON e.Id = pi.EnquiryId
    WHERE pi.Process_Volume_M3h IS NOT NULL
),
DistinctVolumes AS (
    SELECT
        EnquiryId,
        BagfilterMasterId,
        Process_Volume_M3h,
        Enquiry_RequiredBagFilters,
        ROW_NUMBER() OVER (
            PARTITION BY EnquiryId
            ORDER BY Process_Volume_M3h, BagfilterMasterId
        ) AS Qty
    FROM PiBm
    WHERE RnPerVolume = 1
)
SELECT
    dv.EnquiryId,
    dv.BagfilterMasterId,
    dv.Process_Volume_M3h,
    dv.Enquiry_RequiredBagFilters,
    dv.Qty,
    dc.Parameter,
    dc.Value,
    dc.Unit
FROM DistinctVolumes dv
JOIN ionfiltrabagfilters.DamperCostEntity dc
  ON dc.EnquiryId = dv.EnquiryId
 AND dc.BagfilterMasterId = dv.BagfilterMasterId
ORDER BY
    dv.EnquiryId,
    dv.Process_Volume_M3h,
    dv.BagfilterMasterId,
    dc.Id;

    ---cage cost view---
    CREATE OR REPLACE VIEW ionfiltrabagfilters.vw_CageCostDetails AS
WITH PiBm AS (
    SELECT
        e.Id AS EnquiryId,
        bm.BagfilterMasterId,
        pi.Process_Volume_M3h,
        e.RequiredBagFilters AS Enquiry_RequiredBagFilters,
        ROW_NUMBER() OVER (
            PARTITION BY e.Id, pi.Process_Volume_M3h
            ORDER BY bm.BagfilterMasterId
        ) AS RnPerVolume
    FROM ionfiltrabagfilters.ProcessInfo pi
    JOIN ionfiltrabagfilters.BagfilterMaster bm
      ON bm.BagfilterMasterId = pi.BagfilterMasterId
     AND bm.EnquiryId = pi.EnquiryId
    JOIN ionfiltrabagfilters.Enquiry e
      ON e.Id = pi.EnquiryId
    WHERE pi.Process_Volume_M3h IS NOT NULL
),
DistinctVolumes AS (
    SELECT
        EnquiryId,
        BagfilterMasterId,
        Process_Volume_M3h,
        Enquiry_RequiredBagFilters,
        ROW_NUMBER() OVER (
            PARTITION BY EnquiryId
            ORDER BY Process_Volume_M3h, BagfilterMasterId
        ) AS Qty
    FROM PiBm
    WHERE RnPerVolume = 1
)
SELECT
    dv.EnquiryId,
    dv.BagfilterMasterId,
    dv.Process_Volume_M3h,
    dv.Enquiry_RequiredBagFilters,
    dv.Qty,
    cc.Parameter,
    cc.Value,
    cc.Unit
FROM DistinctVolumes dv
JOIN ionfiltrabagfilters.CageCostEntity cc
  ON cc.EnquiryId = dv.EnquiryId
 AND cc.BagfilterMasterId = dv.BagfilterMasterId
ORDER BY
    dv.EnquiryId,
    dv.Process_Volume_M3h,
    dv.BagfilterMasterId,
    cc.Id;
