
---------------Bought-Out Items Inserts--------------------

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'filterBag',           -- frontend logical key
    'Filter Bag',          -- UI label
    'FilterBag',           -- DB table name => used as {viewName}/{tableName}
    1,                     -- section order in UI
    1,                     -- IsActive = true
    JSON_ARRAY(
        JSON_OBJECT('field', 'material', 'label', 'Material', 'type', 'string'),
        JSON_OBJECT('field', 'gsm',      'label', 'GSM',      'type', 'number'),
        JSON_OBJECT('field', 'size',     'label', 'Size',     'type', 'string'),
        JSON_OBJECT('field', 'make',     'label', 'Make',     'type', 'string'),
        JSON_OBJECT('field', 'cost',     'label', 'Cost',     'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'solenoidValve',
    'Solenoid Operated Double Diaphragm Valve',
    'SolenoidValve',       -- DB table name
    2,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'size',  'label', 'Size',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'dpt',
    'Differential Pressure Transmitter',
    'DPTEntity',           -- DB table name
    3,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'dpg',
    'Differential Pressure Gauge',
    'DPGEntity',
    4,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'dps',
    'Differential Pressure Switch',
    'DPSEntity',
    5,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'pg',
    'Pressure Gauge',
    'PGEntity',
    6,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'pt',
    'Pressure Transmitter',
    'PTEntity',
    7,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'ps',
    'Pressure Switch',
    'PSEntity',
    8,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'hld',
  'Hopper Level Detector',
  'HLDEntity',
  9,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'rtd',
  'Resistance Temperature Detector',
  'RTDEntity',
  10,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'utubeManometer',
  'U Tube Manometer',
  'UTubeManometer',
  11,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'explosionVent',
  'Explosion Vent',
  'ExplosionVent',
  12,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','size','label','Size','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'fieldHooter',
  'Field Hooter',
  'FieldHooter',
  13,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','size','label','Size','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'afr',
  'Air Filter Regulator',
  'AFREntity',
  14,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'proxyPulser',
  'Proxy Pulser',
  'ProxyPulser',
  15,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'proximitySwitch',
  'Proximity Switch',
  'ProxymitySwitch',
  16,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'cable',
  'Cable',
  'CableEntity',
  17,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','size','label','Size','type','string'),
    JSON_OBJECT('field','type','label','Type','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'zssController',
  'ZSS Controller',
  'ZssController',
  18,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'junctionBox',
  'Junction Box',
  'JunctionBox',
  19,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'vibrationTransmitter',
  'Vibration Transmitter',
  'VibrationTransmitter',
  20,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','size','label','Size','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'thermocouple',
  'Thermocouple',
  'Thermocouple',
  21,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'thermostat',
  'Thermostat',
  'Thermostat',
  22,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'hopperHeatingPad',
    'Hopper Heating Pad',
    'HopperHeatingPad',
    23,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'hopperHeatingController',
    'Hopper Heating Controller',
    'HopperHeatingcontroller',
    24,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'centrifugalFan',
    'Centrifugal Fan',
    'CentrifualFan',
    25,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',   'label', 'Make',   'type', 'string'),
        JSON_OBJECT('field', 'volume', 'label', 'Volume', 'type', 'string'),
        JSON_OBJECT('field', 'cost',   'label', 'Cost',   'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'screwConveyor',
    'Screw Conveyor',
    'ScrewConveyor',
    26,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',   'label', 'Make',   'type', 'string'),
        JSON_OBJECT('field', 'length', 'label', 'Length', 'type', 'string'),
        JSON_OBJECT('field', 'width',  'label', 'Width',  'type', 'string'),
        JSON_OBJECT('field', 'cost',   'label', 'Cost',   'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'dragChainConveyor',
    'Drag Chain Conveyor',
    'DragChainConveyor',
    27,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',   'label', 'Make',   'type', 'string'),
        JSON_OBJECT('field', 'length', 'label', 'Length', 'type', 'string'),
        JSON_OBJECT('field', 'width',  'label', 'Width',  'type', 'string'),
        JSON_OBJECT('field', 'cost',   'label', 'Cost',   'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'motorisedActuator',
    'Motorised Actuator',
    'MotorisedActuator',
    28,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'ravGearedMotor',
    'RAV Geared Motor',
    'RAVGearedMotor',
    29,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'kw',   'label', 'KW',   'type', 'string'),
        JSON_OBJECT('field', 'make', 'label', 'Make', 'type', 'string'),
        JSON_OBJECT('field', 'cost', 'label', 'Cost', 'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'hardware',
    'Hardware',
    'HardwareEntity',
    30,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'item', 'label', 'Item', 'type', 'string'),
        JSON_OBJECT('field', 'cost', 'label', 'Cost', 'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'sstubing',
    'SS Tubing',
    'SStubing',
    31,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'item',     'label', 'Item',     'type', 'string'),
        JSON_OBJECT('field', 'material', 'label', 'Material', 'type', 'string'),
        JSON_OBJECT('field', 'cost',     'label', 'Cost',     'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'ltMotor',
    'LT Motor',
    'LTMotor',
    32,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'kw',         'label', 'KW',          'type', 'number'),
        JSON_OBJECT('field', 'efficiency', 'label', 'Efficiency',  'type', 'number'),
        JSON_OBJECT('field', 'frameSize',  'label', 'Frame Size',  'type', 'string'),
        JSON_OBJECT('field', 'rpm',        'label', 'RPM',         'type', 'number'),
        JSON_OBJECT('field', 'make',       'label', 'Make',        'type', 'string'),
        JSON_OBJECT('field', 'cost',       'label', 'Cost',        'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'timer',
    'Timer',
    'TimerEntity',
    33,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);



----------------------Cage Costing config inserts -------------

INSERT INTO ionfiltrabagfilters.CageMaterialConfig
(Material, Density, CoilCost, TopBottomCost, VenturiCost, RivetCost)
VALUES
('GI', 7850, 61, 60, 70, 1.5),
('SS304', 7800, 215, 200, 70, 1.5);


INSERT INTO ionfiltrabagfilters.CageMiscellaneousConfig
(Item, Value, Unit)
VALUES
('Packing Raw Material', 0.7, 'Kg'),
('Square Tube for Packing', 60, 'INR'),
('Tarpoline', 2, 'INR/cage');

INSERT INTO ionfiltrabagfilters.AdminCostConfig
(Item, Value, Unit)
VALUES
('Cage Fabrication Cost', 10, 'INR/kg');

INSERT INTO ionfiltrabagfilters.StandardCageConfig
(CageSpec, WeightKg, RawMaterialCost, FixedFabricationCost, BoughtOutCost, PackingCost, PaintCost, TarpolineCost, FinalCostINR)
VALUES
('Cage 10 wire length: 3580 spacing: 150mm coil dia: 4mm', 4.91, 299.51, 65, 131.5, 42, 0.035, 2, 540),
('Cage 10 wire length: 3580 spacing: 175mm coil dia: 4mm', 4.42, 269.62, 55, 131.5, 42, 0.035, 2, 500),
('Cage 10 wire length: 3640 spacing: 150mm coil dia: 4mm', 4.64, 283.04, 65, 131.5, 42, 0.035, 2, 524),
('Cage 10 wire length: 3640 spacing: 175mm coil dia: 4mm', 4.49, 273.89, 65, 131.5, 42, 0.035, 2, 514),

('Cage 12 wire length: 3580 spacing: 150mm coil dia: 4mm', 5.27, 321.47, 78, 131.5, 42, 0.035, 2, 575),
('Cage 12 wire length: 3580 spacing: 175mm coil dia: 4mm', 5.12, 312.32, 69, 131.5, 42, 0.035, 2, 557),
('Cage 12 wire length: 3640 spacing: 150mm coil dia: 4mm', 5.36, 326.96, 78, 131.5, 42, 0.035, 2, 580),
('Cage 12 wire length: 3640 spacing: 175mm coil dia: 4mm', 5.21, 317.81, 78, 131.5, 42, 0.035, 2, 571),

('Cage 14 wire length: 3580 spacing: 175mm coil dia: 4mm', 5.83, 355.63, 70, 131.5, 42, 0.035, 2, 601),
('Cage 14 wire length: 3640 spacing: 150mm coil dia: 4mm', 6.08, 370.88, 91, 131.5, 42, 0.035, 2, 637),
('Cage 14 wire length: 3580 spacing: 150mm coil dia: 4mm', 5.98, 364.78, 91, 131.5, 42, 0.035, 2, 631),
('Cage 14 wire length: 3640 spacing: 175mm coil dia: 4mm', 5.93, 361.73, 91, 131.5, 42, 0.035, 2, 628);


-------------Transportation Rate Config------

INSERT INTO ionfiltrabagfilters.TransportationRateConfig
(LoadingFrom, DestinationState, RatePerKm_40x10x8, DistanceKm)
VALUES
('Pune', 'Andhra Pradesh', 130, 868),
('Pune', 'Arunachal Pradesh', 100, 3062),
('Pune', 'Assam', 100, 2708),
('Pune', 'Bihar', 100, 1717),
('Pune', 'Chhattisgarh', 98, 996),
('Pune', 'Dadra & Nagar Haveli', 272),
('Pune', 'Daman & Diu', 120, 300),
('Pune', 'Delhi', 104, 1462),
('Pune', 'Goa', 110, 469),
('Pune', 'Gujarat', 110, 739),
('Pune', 'Haryana', 105, 1561),
('Pune', 'Himachal Pradesh', 100, 1850),
('Pune', 'Manipur', 100, 3104),
('Pune', 'Jammu & Kashmir', 120, 2102),
('Pune', 'Jharkhand', 102, 1663),
('Pune', 'Karnataka', 125, 521),
('Pune', 'Kerala', 110, 1310),
('Pune', 'Madhya Pradesh', 125, 1077),
('Pune', 'Maharashtra', 115, 388),
('Pune', 'Meghalaya', 90, 2684),
('Pune', 'Nagaland', 90, 3118),
('Pune', 'Odisha', 100, 1445),
('Pune', 'Puducherry', 110, 1185),
('Pune', 'Punjab', 110, 1725),
('Pune', 'Rajasthan', 105, 1258),
('Pune', 'Sikkim', 100, 2289),
('Pune', 'Tamil Nadu', 120, 1166),
('Pune', 'Telangana', 120, 676),
('Pune', 'Uttarakhand', 120, 1791),
('Pune', 'Uttar Pradesh', 115, 1547);



