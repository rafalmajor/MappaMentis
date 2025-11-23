namespace MappaMentis.Domain.Tests;

public class MindMapUnitTests
{
    private MindMap CreateMindMap(string title = "Test Map", string description = "Test Description")
    {
        return new MindMap(Guid.NewGuid(), title, description);
    }

    private MindNode CreateMindNode(Guid mindMapId, string content = "Test Node")
    {
        return new MindNode(Guid.NewGuid(), mindMapId, content);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_with_valid_parameters_should_create_mind_map()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Mind Map";
        var description = "A test mind map";

        // Act
        var mindMap = new MindMap(id, title, description);

        // Assert
        Assert.Equal(id, mindMap.Id);
        Assert.Equal(title, mindMap.Title);
        Assert.Equal(description, mindMap.Description);
        Assert.Empty(mindMap.Nodes);
        Assert.Empty(mindMap.Links);
        Assert.True(mindMap.CreatedAt <= DateTime.UtcNow);
        Assert.True(mindMap.UpdatedAt <= DateTime.UtcNow);
    }

    #endregion

    #region AddNode Tests

    [Fact]
    public void Add_node_with_valid_node_should_add_node_to_mind_map()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node = CreateMindNode(mindMap.Id, "Test Node");

        // Act
        mindMap.AddNode(node);

        // Assert
        Assert.Single(mindMap.Nodes);
        Assert.Contains(node, mindMap.Nodes);
    }

    [Fact]
    public void Add_node_with_null_node_should_throw_argument_null_exception()
    {
        // Arrange
        var mindMap = CreateMindMap();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mindMap.AddNode(null!));
    }

    [Fact]
    public void Add_node_with_duplicate_node_id_should_throw_invalid_operation_exception()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var nodeId = Guid.NewGuid();
        var node1 = new MindNode(nodeId, mindMap.Id, "Node 1");
        var node2 = new MindNode(nodeId, mindMap.Id, "Node 2");

        mindMap.AddNode(node1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => mindMap.AddNode(node2));
    }

    [Fact]
    public void Add_node_should_update_modified_time()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var originalUpdatedAt = mindMap.UpdatedAt;
        var node = CreateMindNode(mindMap.Id);

        System.Threading.Thread.Sleep(10);

        // Act
        mindMap.AddNode(node);

        // Assert
        Assert.True(mindMap.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void Add_node_multiple_nodes_should_add_all_nodes()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var nodes = new[]
        {
            CreateMindNode(mindMap.Id, "Node 1"),
            CreateMindNode(mindMap.Id, "Node 2"),
            CreateMindNode(mindMap.Id, "Node 3")
        };

        // Act
        foreach (var node in nodes)
        {
            mindMap.AddNode(node);
        }

        // Assert
        Assert.Equal(3, mindMap.Nodes.Count);
        foreach (var node in nodes)
        {
            Assert.Contains(node, mindMap.Nodes);
        }
    }

    #endregion

    #region RemoveNode Tests

    [Fact]
    public void Remove_node_with_existing_node_id_should_remove_node()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node = CreateMindNode(mindMap.Id);
        mindMap.AddNode(node);

        // Act
        mindMap.RemoveNode(node.Id);

        // Assert
        Assert.Empty(mindMap.Nodes);
    }

    [Fact]
    public void Remove_node_with_non_existent_node_id_should_not_throw()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var nonExistentId = Guid.NewGuid();

        // Act & Assert - should not throw
        mindMap.RemoveNode(nonExistentId);
    }

    [Fact]
    public void Remove_node_should_remove_associated_links()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node1 = CreateMindNode(mindMap.Id, "Node 1");
        var node2 = CreateMindNode(mindMap.Id, "Node 2");
        var node3 = CreateMindNode(mindMap.Id, "Node 3");

        mindMap.AddNode(node1);
        mindMap.AddNode(node2);
        mindMap.AddNode(node3);

        var link1 = new MindLink(Guid.NewGuid(), mindMap.Id, node1.Id, node2.Id);
        var link2 = new MindLink(Guid.NewGuid(), mindMap.Id, node1.Id, node3.Id);
        var link3 = new MindLink(Guid.NewGuid(), mindMap.Id, node2.Id, node3.Id);

        mindMap.AddLink(link1);
        mindMap.AddLink(link2);
        mindMap.AddLink(link3);

        // Act
        mindMap.RemoveNode(node1.Id);

        // Assert
        Assert.Equal(2, mindMap.Nodes.Count);
        Assert.Single(mindMap.Links); // Only link3 remains
        Assert.Contains(link3, mindMap.Links);
    }

    [Fact]
    public void Remove_node_should_update_modified_time()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node = CreateMindNode(mindMap.Id);
        mindMap.AddNode(node);

        var originalUpdatedAt = mindMap.UpdatedAt;
        System.Threading.Thread.Sleep(10);

        // Act
        mindMap.RemoveNode(node.Id);

        // Assert
        Assert.True(mindMap.UpdatedAt >= originalUpdatedAt);
    }

    #endregion

    #region AddLink Tests

    [Fact]
    public void Add_link_with_valid_link_should_add_link_to_mind_map()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node1 = CreateMindNode(mindMap.Id, "Node 1");
        var node2 = CreateMindNode(mindMap.Id, "Node 2");

        mindMap.AddNode(node1);
        mindMap.AddNode(node2);

        var link = new MindLink(Guid.NewGuid(), mindMap.Id, node1.Id, node2.Id);

        // Act
        mindMap.AddLink(link);

        // Assert
        Assert.Single(mindMap.Links);
        Assert.Contains(link, mindMap.Links);
    }

    [Fact]
    public void Add_link_with_null_link_should_throw_argument_null_exception()
    {
        // Arrange
        var mindMap = CreateMindMap();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mindMap.AddLink(null!));
    }

    [Fact]
    public void Add_link_with_non_existent_source_node_should_throw_invalid_operation_exception()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node = CreateMindNode(mindMap.Id);
        mindMap.AddNode(node);

        var link = new MindLink(Guid.NewGuid(), mindMap.Id, Guid.NewGuid(), node.Id);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => mindMap.AddLink(link));
    }

    [Fact]
    public void Add_link_with_non_existent_target_node_should_throw_invalid_operation_exception()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node = CreateMindNode(mindMap.Id);
        mindMap.AddNode(node);

        var link = new MindLink(Guid.NewGuid(), mindMap.Id, node.Id, Guid.NewGuid());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => mindMap.AddLink(link));
    }

    [Fact]
    public void Add_link_with_duplicate_link_id_should_throw_invalid_operation_exception()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node1 = CreateMindNode(mindMap.Id);
        var node2 = CreateMindNode(mindMap.Id);

        mindMap.AddNode(node1);
        mindMap.AddNode(node2);

        var linkId = Guid.NewGuid();
        var link1 = new MindLink(linkId, mindMap.Id, node1.Id, node2.Id);
        var link2 = new MindLink(linkId, mindMap.Id, node1.Id, node2.Id);

        mindMap.AddLink(link1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => mindMap.AddLink(link2));
    }

    [Fact]
    public void Add_link_should_update_modified_time()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node1 = CreateMindNode(mindMap.Id);
        var node2 = CreateMindNode(mindMap.Id);

        mindMap.AddNode(node1);
        mindMap.AddNode(node2);

        var originalUpdatedAt = mindMap.UpdatedAt;
        System.Threading.Thread.Sleep(10);

        var link = new MindLink(Guid.NewGuid(), mindMap.Id, node1.Id, node2.Id);

        // Act
        mindMap.AddLink(link);

        // Assert
        Assert.True(mindMap.UpdatedAt >= originalUpdatedAt);
    }

    #endregion

    #region RemoveLink Tests

    [Fact]
    public void Remove_link_with_existing_link_id_should_remove_link()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node1 = CreateMindNode(mindMap.Id);
        var node2 = CreateMindNode(mindMap.Id);

        mindMap.AddNode(node1);
        mindMap.AddNode(node2);

        var link = new MindLink(Guid.NewGuid(), mindMap.Id, node1.Id, node2.Id);
        mindMap.AddLink(link);

        // Act
        mindMap.RemoveLink(link.Id);

        // Assert
        Assert.Empty(mindMap.Links);
    }

    [Fact]
    public void Remove_link_with_non_existent_link_id_should_not_throw()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var nonExistentId = Guid.NewGuid();

        // Act & Assert - should not throw
        mindMap.RemoveLink(nonExistentId);
    }

    [Fact]
    public void Remove_link_should_update_modified_time()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var node1 = CreateMindNode(mindMap.Id);
        var node2 = CreateMindNode(mindMap.Id);

        mindMap.AddNode(node1);
        mindMap.AddNode(node2);

        var link = new MindLink(Guid.NewGuid(), mindMap.Id, node1.Id, node2.Id);
        mindMap.AddLink(link);

        var originalUpdatedAt = mindMap.UpdatedAt;
        System.Threading.Thread.Sleep(10);

        // Act
        mindMap.RemoveLink(link.Id);

        // Assert
        Assert.True(mindMap.UpdatedAt >= originalUpdatedAt);
    }

    #endregion

    #region Update Tests

    [Fact]
    public void Update_with_valid_parameters_should_update_title_and_description()
    {
        // Arrange
        var mindMap = CreateMindMap("Old Title", "Old Description");
        var newTitle = "New Title";
        var newDescription = "New Description";

        // Act
        mindMap.Update(newTitle, newDescription);

        // Assert
        Assert.Equal(newTitle, mindMap.Title);
        Assert.Equal(newDescription, mindMap.Description);
    }

    [Fact]
    public void Update_with_null_title_should_throw_argument_null_exception()
    {
        // Arrange
        var mindMap = CreateMindMap();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mindMap.Update(null!, "Description"));
    }

    [Fact]
    public void Update_with_null_description_should_throw_argument_null_exception()
    {
        // Arrange
        var mindMap = CreateMindMap();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mindMap.Update("Title", null!));
    }

    [Fact]
    public void Update_should_update_modified_time()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var originalUpdatedAt = mindMap.UpdatedAt;

        System.Threading.Thread.Sleep(10);

        // Act
        mindMap.Update("New Title", "New Description");

        // Assert
        Assert.True(mindMap.UpdatedAt >= originalUpdatedAt);
    }

    #endregion

    #region Complex Scenarios Tests

    [Fact]
    public void Complex_scenario_create_map_with_nodes_and_links_should_maintain_consistency()
    {
        // Arrange
        var mindMap = CreateMindMap("Project Plan", "A detailed project plan");
        
        var node1 = CreateMindNode(mindMap.Id, "Phase 1");
        var node2 = CreateMindNode(mindMap.Id, "Phase 2");
        var node3 = CreateMindNode(mindMap.Id, "Phase 3");
        var node4 = CreateMindNode(mindMap.Id, "Phase 4");

        // Act - Build the structure
        mindMap.AddNode(node1);
        mindMap.AddNode(node2);
        mindMap.AddNode(node3);
        mindMap.AddNode(node4);

        var link1 = new MindLink(Guid.NewGuid(), mindMap.Id, node1.Id, node2.Id, "leads to");
        var link2 = new MindLink(Guid.NewGuid(), mindMap.Id, node2.Id, node3.Id, "then");
        var link3 = new MindLink(Guid.NewGuid(), mindMap.Id, node3.Id, node4.Id, "finally");

        mindMap.AddLink(link1);
        mindMap.AddLink(link2);
        mindMap.AddLink(link3);

        // Assert
        Assert.Equal(4, mindMap.Nodes.Count);
        Assert.Equal(3, mindMap.Links.Count);

        // Remove a middle node and verify links are cleaned up
        mindMap.RemoveNode(node2.Id);

        Assert.Equal(3, mindMap.Nodes.Count);
        Assert.Equal(1, mindMap.Links.Count); // Only link3 remains
    }

    [Fact]
    public void Complex_scenario_multiple_operations_should_maintain_integrity()
    {
        // Arrange
        var mindMap = CreateMindMap();
        var nodes = new MindNode[5];
        for (int i = 0; i < 5; i++)
        {
            nodes[i] = CreateMindNode(mindMap.Id, $"Node {i + 1}");
            mindMap.AddNode(nodes[i]);
        }

        // Act & Assert - Add and remove multiple links
        var links = new List<MindLink>();
        for (int i = 0; i < 4; i++)
        {
            var link = new MindLink(Guid.NewGuid(), mindMap.Id, nodes[i].Id, nodes[i + 1].Id);
            links.Add(link);
            mindMap.AddLink(link);
        }

        Assert.Equal(4, mindMap.Links.Count);

        // Remove every other link
        mindMap.RemoveLink(links[0].Id);
        mindMap.RemoveLink(links[2].Id);

        Assert.Equal(2, mindMap.Links.Count);

        // Update mind map
        mindMap.Update("Modified Map", "After modifications");
        Assert.Equal("Modified Map", mindMap.Title);
    }

    #endregion
}