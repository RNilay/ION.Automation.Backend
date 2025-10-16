
-- Step 1: Create the database (schema)
CREATE DATABASE ionfiltrabagfilters;

use ionfiltrabagfilters;

-- Step 2: create user
CREATE USER 'IonfiltraUser'@'localhost' IDENTIFIED BY 'Password!@#Db';

-- Step 3: Grant ownership/permissions to your user
GRANT SELECT, INSERT, UPDATE, DELETE ON ionfiltrabagfilters.* TO 'IonfiltraUser'@'localhost';

