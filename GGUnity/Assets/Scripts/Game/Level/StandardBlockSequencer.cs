using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.DebugDescribable;

namespace RJWS.GravGame
{
	public class StandardBlockSequencer: IBlockSequencer
	{
		public class ShapeFreq
		{
			public Shape.AbstractShapeDefn shapeDefn
			{
				get;
				private set;
			}

			public float number
			{
				get;
				private set;
			}

			public ShapeFreq(Shape.AbstractShapeDefn sd, float n)
			{
				shapeDefn = sd;
				number = n;
			}
		}

		private List<ShapeFreq> _shapeFreqs = new List<ShapeFreq>( );

		public StandardBlockSequencer AddShapeToList( Shape.AbstractShapeDefn shape, float number)
		{
			_shapeFreqs.Add( new ShapeFreq( shape, number ) );
			return this;
		}

		private Shape.AbstractShapeDefn GetRandomShapeDefn()
		{
			Shape.AbstractShapeDefn result = null;
			float totalNum = 0f;
			for (int i = 0; i<_shapeFreqs.Count; i++)
			{
				totalNum += _shapeFreqs[i].number;
			}

			float rand = Random.Range( 0f, totalNum );
			for (int i = 0; i<_shapeFreqs.Count; i++)
			{
				if (rand < _shapeFreqs[i].number)
				{
					result = _shapeFreqs[i].shapeDefn;
				}
				else
				{
					rand -= _shapeFreqs[i].number;
				}
			}
			if (result == null)
			{
				Debug.LogWarning( "Failed to randomise from " + _shapeFreqs.Count + " (" + totalNum + ")" );
			}
			return result;
		}

		private int _numberServed = 0;
		private int _numberToServe = 10;

#region IBlockSequencer
		
		public Shape.AbstractShapeDefn GetNextShapeDefn( )
		{
			Shape.AbstractShapeDefn result = null;
			if (false == IsFinished())
			{
				result = GetRandomShapeDefn( );
				_numberServed++;
				Debug.Log( "Served shape #" + _numberServed + " = " + result.DebugDescribe( ) );
			}
			return result;
		}

		public int NumShapes( )
		{
			return _numberToServe;
		}

		public bool IsFinished( )
		{
			return (_numberServed >= _numberToServe);
		}


		public bool Restart( )
		{
			bool success = false;
			if (_numberServed > 0)
			{
				_numberServed = 0;
				success = true;
			}
			else
			{
				Debug.LogWarning( "Restart when already at start" );
			}
			return success;
		}
		#endregion IBlockSequencer

	}

}
