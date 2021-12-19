using _19_BeaconScanner;
using System.Collections.Generic;
using static _19_BeaconScanner.Parser;

const string fileName = @"input.txt";
HashSet<Beacon> allBeacons = TestSmallInput.GetAllUniqueBeacons(fileName);

System.Console.WriteLine(allBeacons.Count);
