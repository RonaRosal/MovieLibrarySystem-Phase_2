// Phase 2
// An implementation of MovieCollection ADT
// 2022
//References:
//Week 5: Lecture & Tutorial (BSTreeADT)
//Week 6: Lecture (Quick Sort)
//To array: BluePixy(2015).Convert binary tree to array in c. Stackoverflow.Retrieved from https://stackoverflow.com/questions/29582431/convert-binary-tree-to-array-in-c



using System;

//A class that models a node of a binary search tree
//An instance of this class is a node in a binary search tree 
public class BTreeNode
{
	private IMovie movie; // movie
	private BTreeNode lchild; // reference to its left child 
	private BTreeNode rchild; // reference to its right child

	public BTreeNode(IMovie movie)
	{
		this.movie = movie;
		lchild = null;
		rchild = null;
	}

	public IMovie Movie
	{
		get { return movie; }
		set { movie = value; }
	}

	public BTreeNode LChild
	{
		get { return lchild; }
		set { lchild = value; }
	}

	public BTreeNode RChild
	{
		get { return rchild; }
		set { rchild = value; }
	}
}

// invariant: no duplicates in this movie collection
public class MovieCollection : IMovieCollection
{
	private BTreeNode root; // movies are stored in a binary search tree and the root of the binary search tree is 'root' 
	private int count; // the number of (different) movies currently stored in this movie collection 



	// get the number of movies in this movie colllection 
	// pre-condition: nil
	// post-condition: return the number of movies in this movie collection and this movie collection remains unchanged
	public int Number { get { return count; } }

	// constructor - create an object of MovieCollection object
	public MovieCollection()
	{
		root = null;
		count = 0;	
	}

	// Check if this movie collection is empty
	// Pre-condition: nil
	// Post-condition: return true if this movie collection is empty; otherwise, return false.
	public bool IsEmpty()
	{
		
		return root == null;
	}

	// Insert a movie into this movie collection
	// Pre-condition: nil
	// Post-condition: the movie has been added into this movie collection and return true, if the movie is not in this movie collection; otherwise, the movie has not been added into this movie collection and return false.
	public bool Insert(IMovie movie)
	{
		if (root == null)
		{
			root = new BTreeNode(movie);
			count++;
			return true;
		}

		bool inserted = Insert(movie, root);

		if (inserted == true)
			count++;

		return inserted; 
	}

	//Create Private Method for insert
	//pre: ptr != null
	//post: movie is inserted to the binary search tree rooted at ptr
	private bool Insert(IMovie movie, BTreeNode ptr)
	{
		if (movie.CompareTo(ptr.Movie) < 0)
		{
			if (ptr.LChild == null)
			{
				ptr.LChild = new BTreeNode(movie);
				return true;
			}
			else
				return Insert(movie, ptr.LChild);
		}
		else if (movie.CompareTo(ptr.Movie) > 0)
		{
			if (ptr.RChild == null)
			{
				ptr.RChild = new BTreeNode(movie);
				return true;
			}
			else
				return Insert(movie, ptr.RChild);
		}

		return false;
	}


	// Delete a movie from this movie collection
	// Pre-condition: nil
	// Post-condition: the movie is removed out of this movie collection and return true, if it is in this movie collection; return false, if it is not in this movie collection
	public bool Delete(IMovie movie)
	{
		if (movie == null)
			return false;

		BTreeNode ptr = root;
		BTreeNode parent = null; ;

		while ((ptr != null) && (movie.CompareTo(ptr.Movie) != 0))
		{
			parent = ptr;
			if (movie.CompareTo(ptr.Movie) < 0)
				ptr = ptr.LChild;
			else
				ptr = ptr.RChild;
		}

		if (ptr != null)
		{
			if ((ptr.LChild != null) && (ptr.RChild != null))
			{
				if (ptr.LChild.RChild == null)
				{
					ptr.Movie = ptr.LChild.Movie;
					ptr.LChild = ptr.LChild.LChild;
				}
				else
				{
					BTreeNode p = ptr.LChild;
					BTreeNode pp = ptr;

					while (p.RChild != null)
					{
						pp = p;
						p = p.RChild;


					}

					ptr.Movie = p.Movie;
					pp.RChild = p.LChild;
				}

				count--;
				return true;
			}
			else
			{
				BTreeNode c;
				if (ptr.LChild != null)
					c = ptr.LChild;
				else
					c = ptr.RChild;

				if (ptr == root)
					root = c;
				else
				{
					if (ptr == parent.LChild)
						parent.LChild = c;
					else
						parent.RChild = c;
				}

				count--;
				return true;
			}

		}
		return false;
		
	}

	// Search for a movie in this movie collection
	// pre: nil
	// post: return true if the movie is in this movie collection;
	//	     otherwise, return false.
	public bool Search(IMovie movie)
	{
		if (movie == null)
			return false;
		return Search(movie, root);
	}

	//Create private method for search
	private bool Search(IMovie movie, BTreeNode r)
	{
		if (r == null) 
			return false;

		if (movie.CompareTo(r.Movie) == 0)
			return true;
		else if (movie.CompareTo(r.Movie) < 0)
			return Search(movie, r.LChild);
		else
			return Search(movie, r.RChild);
	}

	// Search for a movie by its title in this movie collection  
	// pre: nil
	// post: return the reference of the movie object if the movie is in this movie collection;
	//	     otherwise, return null.
	public IMovie Search(string movietitle)
	{
		if (movietitle == null)
			return null;

		return Search(movietitle, root);
	}

	//Create private method for search
	private IMovie Search(string movietitle, BTreeNode r)
	{
		if (r == null)
			return null;

		if (movietitle.CompareTo(r.Movie.Title) == 0)
			return r.Movie;
		else if (movietitle.CompareTo(r.Movie.Title) < 0)
			return Search(movietitle, r.LChild);
		else
			return Search(movietitle, r.RChild);
	}

	// Store all the movies in this movie collection in an array in the dictionary order by their titles
	// Pre-condition: nil
	// Post-condition: return an array of movies that are stored in dictionary order by their titles
	public IMovie[] ToArray()
	{
		IMovie[] movies= new Movie[Number]; // List of movies 
		AddMovieToArray(root, movies, 0); // Add to array movies
		QuickSort(movies, 0, Number - 1); // sort the array
		return movies;
	}

	//Convert BSTreeNode to Array  Add to movies[]
	private int AddMovieToArray(BTreeNode r, IMovie[] movies, int index)
	{
		if (r.LChild != null)
		{
			index = AddMovieToArray(r.LChild, movies, index);
		
		}

		if (r.RChild != null)
		{
			index = AddMovieToArray(r.RChild, movies, index);
		
		}

		movies[index] = r.Movie;

		return index + 1;
	}

	//Implement Quick Sort Algorithm
	//Sort movies in dictionary order

	private void QuickSort(IMovie[] movies, int left, int right)
	{
		int pivot;

		if (left < right)
		{
			pivot = QuickSortPartition(movies, left, right);

			if (pivot > 1)
			{
				QuickSort(movies, left, pivot - 1);
			}
			if (pivot + 1 < right)
			{
				QuickSort(movies, pivot + 1, right);
			}
		}
	}


	private int QuickSortPartition(IMovie[] movies, int left, int right)
	{
		IMovie movie = movies[left];

		while (true)
		{
			while (movies[left].CompareTo(movie) < 0)
			{
				left++;
			}
			while (movies[left].CompareTo(movie) > 0)
			{
				right--;
			}

			if (left < right)
			{
				IMovie temp = movies[right];
				movies[right] = movies[left];
				movies[left] = temp;
			}
			else 
			{
				return right;
			
			}
		
		
		}
	
	}

	// Clear this movie collection
	// Pre-condotion: nil
	// Post-condition: all the movies have been removed from this movie collection 
	public void Clear()
	{
		root = null;
		count = 0;
	}
}





