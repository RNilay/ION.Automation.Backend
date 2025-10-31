
---Enquiry table

CREATE TABLE Enquiry (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    EnquiryId VARCHAR(255) NOT NULL,
    Customer TEXT,
    RequiredBagFilters INT,
    ProcessVolumes INT,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE (EnquiryId)
);

--Assignment table
CREATE TABLE AssignmentEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EnquiryId VARCHAR(255) NOT NULL,
    EnquiryAssignmentId TEXT,
    Customer TEXT,
    ProcessVolumes INT,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry (EnquiryId) ON DELETE CASCADE
);

CREATE TABLE
    ionfiltrabagfilters.BagfilterMaster (
        BagfilterMasterId INT AUTO_INCREMENT PRIMARY KEY,
        AssignmentId INT NULL,
        EnquiryId INT NULL,
        BagFilterName TEXT,
        Status TEXT NULL,
        Revision INT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP
    );



    CREATE TABLE
    ionfiltrabagfilters.BagfilterInput (
        BagfilterInputId INT AUTO_INCREMENT PRIMARY KEY,
        BagfilterMasterId INT NOT NULL,
        EnquiryId INT NULL,
        Process_Volume_M3h DECIMAL(10, 2),
        Location TEXT,
        Process_Dust TEXT,
        Process_Dustload_Gmspm3 DECIMAL(10, 2),
        Process_Temp_C DECIMAL(10, 2),
        Dew_Point_C DECIMAL(10, 2),
        Outlet_Emission_Mgpm3 TEXT,
        Process_Cloth_Ratio DECIMAL(10, 2),
        Specific_Gravity DECIMAL(10, 2),
        Customer_Equipment_Tag_No TEXT,
        Bagfilter_Cleaning_Type TEXT,
        Offline_Maintainence TEXT,
        Cage_Wire_Dia DECIMAL(10, 2),
        No_Of_Cage_Wires DECIMAL(10, 2),
        Ring_Spacing DECIMAL(10, 2),
        Filter_Bag_Dia DECIMAL(10, 2),
        Fil_Bag_Length DECIMAL(10, 2),
        Fil_Bag_Recommendation TEXT,
        Gas_Entry TEXT,
        Support_Structure_Type TEXT,
        Can_Correction DECIMAL(10, 2),
        Valve_Size DECIMAL(10, 2),
        Voltage_Rating TEXT,
        Cage_Type TEXT,
        Cage_Length TEXT,
        Capsule_Height DECIMAL(10, 2),
        Tube_Sheet_Thickness DECIMAL(10, 2),
        Capsule_Wall_Thickness DECIMAL(10, 2),
        Canopy TEXT,
        Solenoid_Valve_Maintainence TEXT,
        Casing_Wall_Thickness DECIMAL(10, 2),
        Hopper_Type TEXT,
        Process_Compartments DECIMAL(10, 2),
        Tot_No_Of_Hoppers DECIMAL(10, 2),
        Tot_No_Of_Trough DECIMAL(10, 2),
        Plenum_Width DECIMAL(10, 2),
        Inlet_Height DECIMAL(10, 2),
        Hopper_Thickness DECIMAL(10, 2),
        Hopper_Valley_Angle DECIMAL(10, 2),
        Access_Door_Type TEXT,
        Access_Door_Qty DECIMAL(10, 2),
        Rav_Maintainence_Pltform TEXT,
        Hopper_Access_Stool TEXT,
        Is_Distance_Piece TEXT,
        Distance_Piece_Height DECIMAL(10, 2),
        Stiffening_Factor DECIMAL(10, 2),
        Hopper DECIMAL(10, 2),
        Discharge_Opening_Sqr DECIMAL(10, 2),
        Material_Handling TEXT,
        Material_Handling_Qty DECIMAL(10, 2),
        Trough_Outlet_Length DECIMAL(10, 2),
        Trough_Outlet_Width DECIMAL(10, 2),
        Material_Handling_XXX TEXT,
        Support_Struct_Type TEXT,
        No_Of_Column DECIMAL(10, 2),
        Ground_Clearance DECIMAL(10, 2),
        Access_Type TEXT,
        Cage_Weight_Ladder DECIMAL(10, 2),
        Mid_Landing_Pltform TEXT,
        Platform_Weight DECIMAL(10, 2),
        Staircase_Height DECIMAL(10, 2),
        Staircase_Weight DECIMAL(10, 2),
        Railing_Weight DECIMAL(10, 2),
        Maintainence_Pltform TEXT,
        Maintainence_Pltform_Weight DECIMAL(10, 2),
        Blow_Pipe DECIMAL(10, 2),
        Pressure_Header DECIMAL(10, 2),
        Distance_Piece DECIMAL(10, 2),
        Access_Stool_Size_Mm DECIMAL(10, 2),
        Access_Stool_Size_Kg DECIMAL(10, 2),
        Roof_Door_Thickness DECIMAL(10, 2),
        Bag_Per_Row DECIMAL(10, 2),
        Number_Of_Rows DECIMAL(10, 2),
        IsMatched BOOLEAN NOT NULL DEFAULT 0,
        MatchedBagfilterInputId INT NULL,
        MatchedBagfilterMasterId INT NULL,
        MatchedAt DATETIME NULL,
        S3dModel JSON NULL,
        AnalysisResult JSON NULL,
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.BagfilterMaster (BagfilterMasterId) ON DELETE CASCADE
    );

    -- index for the above table:
    CREATE INDEX IX_BagfilterInputs_CompositeKey
ON BagfilterInput (Location(100), No_Of_Column, Ground_Clearance, Bag_Per_Row, Number_Of_Rows);


--- Master Table for Database of Without Canpoy Bagfilters

CREATE TABLE
  `IFI_Bagfilter_Database_Without_Canopy` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Process_Volume_m3hr` TEXT,
    `Hopper_type` TEXT,
    `Number_of_columns` DECIMAL(10, 2),
    `Number_of_bays_in_X_direction` DECIMAL(10, 2),
    `Number_of_bays_in_Y_direction` DECIMAL(10, 2),
    `Column_CC_distance_in_X_direction_mm` DECIMAL(10, 2),
    `Column_CC_distance_in_Y_direction_mm` DECIMAL(10, 2),
    `Clearance_Below_Hopper_Flange_mm` DECIMAL(10, 2),
    `Height_upto_mm_Column` DECIMAL(10, 2),
    `Height_upto_mm_Tube_Sheet` DECIMAL(10, 2),
    `Height_upto_mm_Capsule_Top` DECIMAL(10, 2),
    `Member_Sizes_Column` TEXT,
    `Member_Sizes_Beam` TEXT,
    `Member_Sizes_Bracing_and_Ties` TEXT,
    `Member_Sizes_RAV` TEXT,
    `Member_Sizes_Staging_Beam` TEXT,
    `Member_Sizes_Grid_Beam` TEXT,
    `Bolts_No_of_Bolt` DECIMAL(10, 2),
    `Bolts_Dia_of_Bolt` DECIMAL(10, 2),
    `Bolts_Grade_of_Bolt` DECIMAL(10, 2),
    `Bolts_Sleeve_Size_mm` TEXT,
    `Bolts_Embedded_Length_mm` DECIMAL(10, 2),
    `Bolt_CC_Distance_Confirguration_RCC` TEXT,
    `Bolt_CC_Distance_Confirguration_Steel` TEXT,
    `Base_Plate_Dimension_RCC` TEXT,
    `Base_Plate_Dimension_Steel` TEXT,
    `Weight_of_Base_Plate_kg` DECIMAL(10, 2),
    `Total_Weight_of_Structure_kg` DECIMAL(10, 2),
    `CreatedAt` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
  );