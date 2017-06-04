using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.GravGame
{
	public class Block : MonoBehaviour
	{
		public MeshFilter meshFilter;
		public MeshRenderer meshRenderer;
		public MeshCollider meshCollider;
		public Rigidbody cachedRB
		{
			get;
			private set;
		}

		public Transform cachedTransform
		{
			get;
			private set;
		}

		private BlockDefinition _blockDefn = null;
		
		private void Awake()
		{
			cachedTransform = transform;
			cachedRB = GetComponent<Rigidbody>( );
			if (meshFilter == null)
			{
				meshFilter = gameObject.AddComponent<MeshFilter>( );
			}
			if (meshCollider == null)
			{
				meshCollider = gameObject.AddComponent<MeshCollider>( );
			}
			if (meshRenderer == null)
			{
				meshRenderer = gameObject.AddComponent<MeshRenderer>( );
			}

		}

		public void MakeStatic(Material mat)
		{
			cachedRB.useGravity = false;
			cachedRB.isKinematic = true;
			cachedRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			SetLayer( "Block" );
			SetMaterial( mat );
		}

		public void MakeFree( Material mat )
		{
			cachedRB.useGravity = true;
			cachedRB.isKinematic = false;
			cachedRB.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
			SetLayer( "Block" );
			SetMaterial( mat );
		}

		public void MakeSelectMode( Material mat )
		{
			SetLayer( "Effects" );
			cachedRB.isKinematic = true;
			cachedRB.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
			SetMaterial( mat );
		}

		public void Init(BlockDefinition bd)
		{
			_blockDefn = bd;
			CreateMesh( );
		}

		public void SetMaterial(Material m)
		{
			meshRenderer.sharedMaterial = m;
		}

		public void SetLayer(string layerName)
		{
			gameObject.layer = AppManager.Instance.GetLayerIndex( layerName );
		}

		public void SetPhysMaterial( PhysicMaterial m )
		{
			meshCollider.sharedMaterial = m;
		}

		static readonly float defaultResolution = 0.05f;
		static readonly float zDepth = 2f;

		private void CreateMesh()
		{
			Vector2 centreXY = Vector2.zero;

			List<Vector3> verts = new List<Vector3>( );
			List<Vector2> uvs = new List<Vector2>( );
			List<Vector3> normals = new List<Vector3>( );

			List<int> tris = new List<int>( );

			List<Vector2> edgePoints = _blockDefn.shape.GetEdgePoints( centreXY, defaultResolution);

			int numEdgePoints = edgePoints.Count;

			float zNear = -0.5f * zDepth;
			float zFar = 0.5f * zDepth;

			// near centre
			int index_nearCentre = verts.Count;

			Vector3 norm = Vector3.forward;

			verts.Add( new Vector3( centreXY.x, centreXY.y, zNear ) );
			uvs.Add( new Vector2( 0.0f, 0.0f ) );
			normals.Add( norm );

			// near hoop

			int index_nearHoop0 = verts.Count;
			for (int i = 0; i < numEdgePoints; i++)
			{
				verts.Add( new Vector3( edgePoints[i].x, edgePoints[i].y, zNear ) );
				uvs.Add( new Vector2( 1f, ((float)i)/numEdgePoints ) );
				normals.Add( norm );
			}

			// far centre
			int index_farCentre = verts.Count;

			norm = Vector3.back;

			verts.Add( new Vector3( centreXY.x, centreXY.y, zFar) );
			uvs.Add( new Vector2( 0.0f, 0.0f ) );
			normals.Add( norm );

			// far hoop

			int index_farHoop0 = verts.Count;
			for (int i = 0; i < numEdgePoints; i++)
			{
				verts.Add( new Vector3( edgePoints[i].x, edgePoints[i].y, zFar) );
				uvs.Add( new Vector2( 1f, ((float)i) / numEdgePoints ) );
				normals.Add( norm );
			}

			AddTrisForEnd( tris, numEdgePoints, index_nearCentre, index_nearHoop0, false );
			AddTrisForEnd( tris, numEdgePoints, index_farCentre, index_farHoop0, true );
			AddTrisForSides( tris, numEdgePoints, index_nearHoop0, index_farHoop0 );

			Mesh mesh = meshFilter.sharedMesh;
			if (mesh == null)
			{
				meshFilter.sharedMesh = new Mesh( );
				mesh = meshFilter.sharedMesh;
			}
			mesh.Clear( );

			mesh.vertices = verts.ToArray( );
			mesh.triangles = tris.ToArray( );
			mesh.uv = uvs.ToArray( );
			mesh.normals = normals.ToArray( );

			mesh.RecalculateBounds( );
			mesh.RecalculateNormals( );

			meshCollider.sharedMesh = mesh;

		}

		private void AddTrisForSides( List<int> tris, int numEdgePoints, int nearEdgePoint0Index, int farEdgePoint0Index )
		{
			for (int index = 0; index < numEdgePoints; index++)
			{
				int p1NearIndex = nearEdgePoint0Index + index;
				int p2NearIndex = nearEdgePoint0Index + ( index + 1) % numEdgePoints;

				int p1FarIndex = farEdgePoint0Index + index;
				int p2FarIndex = farEdgePoint0Index + (index + 1) % numEdgePoints;

				tris.Add( p1NearIndex );
				tris.Add( p2FarIndex );
				tris.Add( p2NearIndex );

				tris.Add( p1NearIndex );
				tris.Add( p1FarIndex );
				tris.Add( p2FarIndex );
			}
		}

		private void AddTrisForEnd( List<int> tris, int numEdgePoints, int centreIndex, int edgePoint0index, bool reverse)
		{
			for (int index = 0; index < numEdgePoints; index++)
			{
				tris.Add( centreIndex );

				int p1Index = edgePoint0index + index;
				int p2Index = edgePoint0index + (index+1) % numEdgePoints;

				if (reverse)
				{
					tris.Add( p2Index );
					tris.Add( p1Index );
				}
				else
				{
					tris.Add( p1Index );
					tris.Add( p2Index );
				}
			}
		}
	}

}
