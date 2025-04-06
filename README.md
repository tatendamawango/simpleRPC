# ⚙️ SimpleRPC Container Simulation

This project demonstrates a distributed client-server simulation using [SimpleRPC](https://github.com/DaniilSokolyuk/SimpleRpc), a lightweight RPC framework in C#. The system models a pressurized container where **input and output clients** modify the container's mass, affecting pressure and triggering simulation resets based on pressure limits.

---

## 🧩 Project Structure

```
├── Server.cs               # Hosts the container simulation RPC service
├── Client.cs               # Acts as the input/output client
├── ContainerService.cs     # Exposes IContainerService methods
├── ContainerLogic.cs       # Core physics logic (mass, temp, pressure)
├── IContainerService.cs    # Interface for container operations
```

---

## 🔧 Features

- ✅ **Thread-safe container simulation**
- ✅ **Client-server interaction via SimpleRPC**
- ✅ **RPC methods exposed:**
  - `SetMass(double mass)`
  - `GetContainerLimits()`
  - `ContainerInfo()`
  - `ActiveClient()` → tells client whether to add/remove gas
- ✅ **Pressure monitoring and reset:**
  - Explodes if pressure > 100,000 Pa
  - Implodes if pressure < 70,000 Pa
- ✅ **Automatic recovery from exceptions**
- ✅ **Logging with NLog**

---

## 🧪 How It Works

- The **Server** runs the simulation and exposes methods via SimpleRPC.
- The **Input Client** adds mass (gas) when pressure is too low.
- The **Output Client** removes mass when pressure is too high.
- **Container Logic** updates temperature and pressure every 2 seconds.
- If limits are exceeded, the system logs the event and resets the simulation.

---

## 🖥️ Example Output (Console Logs)

```
13:00:01|INFO| Input client actives
13:00:03|INFO| Input client working. Pressure was 81234.5
13:00:05|INFO| New temperature is 278.5 Change: +5.2
13:00:05|INFO| New Mass is 45.1    Mass change: +4.6
13:00:05|INFO| New Pressure is 97000.3
...
```

---

## 🚀 How to Run

### 🖥 Server
1. Open the solution in Visual Studio or via CLI.
2. Run `Server.cs`.

### 🧑‍💻 Client
1. Open and run `Client.cs`.
2. Adjust `ActiveClient()` logic to differentiate between input/output clients:
   - Input client: runs when `ActiveClient() == 1`
   - Output client: runs when `ActiveClient() == 2`

> 💡 You can run two instances of the client with different logic to simulate both behaviors simultaneously.

---

## 🔗 Dependencies

- [.NET 6.0+](https://dotnet.microsoft.com/)
- [SimpleRPC](https://github.com/kekekeks/SimpleRpc)
- [NLog](https://nlog-project.org/)
