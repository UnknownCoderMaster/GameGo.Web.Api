using System;

namespace GameGo.Domain.ValueObjects;

public class Coordinate : IEquatable<Coordinate>
{
	public decimal Latitude { get; private set; }
	public decimal Longitude { get; private set; }

	private Coordinate() { }

	public Coordinate(decimal latitude, decimal longitude)
	{
		if (latitude < -90 || latitude > 90)
			throw new ArgumentException("Latitude must be between -90 and 90", nameof(latitude));

		if (longitude < -180 || longitude > 180)
			throw new ArgumentException("Longitude must be between -180 and 180", nameof(longitude));

		Latitude = latitude;
		Longitude = longitude;
	}

	public double DistanceTo(Coordinate other)
	{
		// Haversine formula
		const double earthRadiusKm = 6371;

		var lat1 = (double)Latitude * Math.PI / 180;
		var lat2 = (double)other.Latitude * Math.PI / 180;
		var lon1 = (double)Longitude * Math.PI / 180;
		var lon2 = (double)other.Longitude * Math.PI / 180;

		var dLat = lat2 - lat1;
		var dLon = lon2 - lon1;

		var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
				Math.Cos(lat1) * Math.Cos(lat2) *
				Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

		var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

		return earthRadiusKm * c;
	}

	public bool Equals(Coordinate other)
	{
		if (other is null) return false;
		return Latitude == other.Latitude && Longitude == other.Longitude;
	}

	public override bool Equals(object obj) => Equals(obj as Coordinate);

	public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);
}