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





