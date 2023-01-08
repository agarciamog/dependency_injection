using TheDependencyProblem.CarExample;

var dieselCar = new Car(new DieselEngine());
var petrolCar = new Car(new PetrolEngine());

var testCar = new Car(new TestEngine());