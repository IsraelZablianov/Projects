package hit.memoryunits;

import java.io.Serializable;

public class Page<T> implements Serializable
{
	private T content;
	private Long ID;
	
	public Page() 
	{		
	}
	
	public Page(Long ID,T content) 
	{
		this.ID = ID;
		this.content = content;
	}
	
	public long getPageId() 
	{
		return ID;
	}
	
	public void setPageId(long pageId) 
	{
		ID=pageId;
	}
	
	public T getContent() 
	{
		return content;
	}
	
	public void setContent(T content) 
	{
		this.content=content;
	}
	
	@Override
	public int hashCode()
	{		
		return	ID.intValue();		
	}
	
	@Override
	public boolean equals(Object obj)
	{
        if (obj == this) 
        {
            return true;
        }
        
        if (!(obj instanceof Page)) 
        {
            return false;
        }
        
		Page object = (Page)obj;
		return object.getPageId() == ID;		
	}
	
	@Override
	public String toString()
	{
		return "("+ID+","+content.toString()+")";
	}		
}
