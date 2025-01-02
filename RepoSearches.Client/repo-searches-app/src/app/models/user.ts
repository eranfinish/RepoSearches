export class User {
  id: number = 0;
  name: string = "";
  email: string = "";
  isRegistering: boolean = false;
  role: string = "";
  createdAt: string = new Date().toUTCString();
  updatedAt: string = new Date().toUTCString()
}
