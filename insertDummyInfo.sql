/* Insert some dummy records */

/* Basic records - do not need foreign keys */
INSERT INTO Component
(name, cost) VALUES
('Big Rat', 99.99)
, ('Small Rat', 49.99)
, ('Micro Rat', 19.99)
, ('Rat Food, 1lb', 4.99)
, ('Rat Cage, Small', 19.99);

INSERT INTO Product
(name) VALUES
('Big Rat')
, ('Small Rat')
, ('Micro Rat')
, ('Rat Package Deal')
, ('Rat Soup, 5lb');

INSERT INTO Location
(address) VALUES
('1600 Pennsylvania Ave NW, Washington, DC 20500') /* White House */
, ('12500 TI Boulevard Dallas, TX 75243') /* Texas Instruments */
, ('410 Terry Ave N, Seattle, WA 98109'); /* Amazon HQ */

/* Get the tables we just populated for the ids */
SELECT * FROM Component;
SELECT * FROM Product;
SELECT * FROM Location;

/* Tables which require some manual connecting for the foreign keys */

INSERT INTO ProductComponent
(productId, componentId, quantity) VALUES
(1, 1, 1)
, (2, 2, 1)
, (3, 3, 1)
/*Rat Package Deal is a Micro Rat, two pounds of food, and a cage */
, (4, 3, 1)
, (4, 4, 2)
, (4, 5, 1)
/* Rat Soup is three Small Rats and a pound of food */
, (5, 2, 3)
, (5, 4, 1);

INSERT INTO Inventory
(locationId, componentId, quantity) VALUES
(1, 3, 10)
, (1, 1, 1)
, (2, 1, 3)
, (2, 2, 10)
, (2, 3, 100)
, (3, 3, 30)
, (3, 4, 10)
, (4, 1, 10)
, (4, 2, 10)
, (4, 3, 10)
, (4, 4, 10)
, (4, 5, 10);