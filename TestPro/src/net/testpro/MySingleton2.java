package net.testpro;

/*
 * Multithreading not supported
 */
public class MySingleton2 {

	private static MySingleton2 _instance;

	private MySingleton2() {
		// construct object . . .
	}

	// For lazy initialization
	public static synchronized MySingleton2 getInstance() {
		if (_instance == null) {
			_instance = new MySingleton2();
		}
		return _instance;
	}
	// Remainder of class definition . . .

}
