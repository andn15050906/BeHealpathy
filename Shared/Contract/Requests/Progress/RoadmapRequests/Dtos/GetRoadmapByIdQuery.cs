using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.RoadmapRequests.Dtos
{
    public sealed class GetRoadmapByIdQuery : GetByIdQuery<RoadmapModel>
    {
        public GetRoadmapByIdQuery(Guid id) : base(id)
        {
        }
    }
}
